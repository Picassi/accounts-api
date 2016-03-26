using System;
using System.Linq;
using Picassi.Core.Accounts.DbAccess.Snapshots;
using Picassi.Core.Accounts.DbAccess.Transactions;
using Picassi.Core.Accounts.ViewModels.Accounts;
using Picassi.Core.Accounts.ViewModels.Transactions;

namespace Picassi.Core.Accounts.Reports
{
    public interface IAccountBalanceService
    {
        decimal GetAccountBalance(int accountId, DateTime date);
    }

    public class AccountBalanceService : IAccountBalanceService
    {
        private readonly ISnapshotQueryService _snapshotQueryService;
        private readonly ITransactionQueryService _transactionQueryService;

        public AccountBalanceService(ISnapshotQueryService snapshotQueryService, ITransactionQueryService transactionQueryService)
        {
            _snapshotQueryService = snapshotQueryService;
            _transactionQueryService = transactionQueryService;
        }

        public decimal GetAccountBalance(int accountId, DateTime date)
        {
            var snapshot = _snapshotQueryService.GetLastSnapshotBefore(accountId, date);
            var period = new AccountPeriodViewModel { AccountId = accountId, From = snapshot?.Date, To = date };
            var transactionTotal = TransactionTotal(accountId, period);
            return (snapshot?.Amount ?? 0) + transactionTotal;
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
