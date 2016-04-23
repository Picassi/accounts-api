using System;
using System.Linq;
using Picassi.Core.Accounts.DbAccess.Snapshots;
using Picassi.Core.Accounts.DbAccess.Transactions;
using Picassi.Core.Accounts.ViewModels.Accounts;
using Picassi.Core.Accounts.ViewModels.Transactions;
using Picassi.Data.Accounts.Database;

namespace Picassi.Core.Accounts.Reports
{
    public interface IAccountBalanceService
    {
        decimal GetAccountBalance(int accountId, DateTime date);
        void SetTransactionBalances(int accountId, DateTime date);
    }

    public class AccountBalanceService : IAccountBalanceService
    {
        private readonly IAccountsDataContext _dataContext;
        private readonly ISnapshotQueryService _snapshotQueryService;
        private readonly ITransactionQueryService _transactionQueryService;

        public AccountBalanceService(ISnapshotQueryService snapshotQueryService, ITransactionQueryService transactionQueryService, IAccountsDataContext dataContext)
        {
            _snapshotQueryService = snapshotQueryService;
            _transactionQueryService = transactionQueryService;
            _dataContext = dataContext;
        }

        public decimal GetAccountBalance(int accountId, DateTime date)
        {
            var snapshot = _snapshotQueryService.GetLastSnapshotBefore(accountId, date);
            var period = new AccountPeriodViewModel { AccountId = accountId, From = snapshot?.Date, To = date };
            var transactionTotal = TransactionTotal(accountId, period);
            return (snapshot?.Amount ?? 0) + transactionTotal;
        }

        public void SetTransactionBalances(int accountId, DateTime date)
        {
            var initialBalance = GetAccountBalance(accountId, date);
            var nextSnapshot = _dataContext.Snapshots.Where(x => x.AccountId == accountId && x.Date > date)
                    .OrderByDescending(x => x.Date).FirstOrDefault();
            var endDate = nextSnapshot?.Date;

            var transactions = _dataContext.Transactions.Where(x => x.Date >= date);
            if (endDate != null) transactions = transactions.Where(x => x.Date < (DateTime) endDate);
            transactions = transactions.Where(x => x.FromId == accountId || x.ToId == accountId).OrderBy(x => x.Date).ThenBy(x => x.Id);

            foreach (var transaction in transactions)
            {                
                if (transaction.FromId == accountId)
                    initialBalance = initialBalance - transaction.Amount;
                else if (transaction.ToId == accountId)
                    initialBalance = initialBalance + transaction.Amount;
                transaction.Balance = initialBalance;
            }
            _dataContext.SaveChanges();
        }

        private decimal TransactionTotal(int accountId, AccountPeriodViewModel period)
        {
            var query = new SimpleTransactionQueryModel { AccountId = accountId, DateFrom = period.From, DateTo = period.To };
            var transactions = _transactionQueryService.Query(query).ToList();
            var totalCredited = transactions.Where(x => x.ToId == accountId).Select(x => x.Amount).DefaultIfEmpty(0).Sum();
            var totalDebited = transactions.Where(x => x.FromId == accountId).Select(x => x.Amount).DefaultIfEmpty(0).Sum();
            return totalCredited - totalDebited;
        }
    }
}
