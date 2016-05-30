using System;
using System.Linq;
using Picassi.Core.Accounts.DbAccess.Snapshots;
using Picassi.Core.Accounts.DbAccess.Transactions;
using Picassi.Core.Accounts.ViewModels.Accounts;
using Picassi.Core.Accounts.ViewModels.Transactions;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.Reports
{
    public interface IAccountBalanceService
    {
        decimal GetAccountBalance(int accountId, DateTime date);
        void SetTransactionBalances(int accountId, DateTime date);
        void SetTransactionBalances(int accountId, DateTime date, Transaction transaction, decimal initialBalance);
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
            var transaction = GetFirstTransactionFromDate(accountId, date) ?? GetFirstTransactionBeforeDate(accountId, date);

            SetTransactionBalances(accountId, date, transaction, initialBalance);
        }

        private Transaction GetFirstTransactionBeforeDate(int accountId, DateTime date)
        {
            return _dataContext.Transactions
                .Where(x => x.FromId == accountId || x.ToId == accountId)
                .Where(x => x.Date < date)
                .OrderByDescending(x => x.Date).ThenByDescending(x => x.Id)
                .FirstOrDefault();
        }

        private Transaction GetFirstTransactionFromDate(int accountId, DateTime date)
        {
            return _dataContext.Transactions
                .Where(x => x.FromId == accountId || x.ToId == accountId)
                .Where(x => x.Date >= date)
                .OrderBy(x => x.Date).ThenBy(x => x.Id)
                .FirstOrDefault();
        }

        public void SetTransactionBalances(int accountId, DateTime date, Transaction transaction, decimal initialBalance)
        {
            if (transaction == null) return;

            SetBalanceForward(GetTransactionsAfterTransaction(accountId, transaction), accountId, initialBalance);
            SetBalanceBackwards(GetTransactionBeforeTransaction(accountId, transaction), accountId, initialBalance);

            _dataContext.SaveChanges();
        }

        private IQueryable<Transaction> GetTransactionsAfterTransaction(int accountId, Transaction transaction)
        {
            return _dataContext.Transactions                
                .Where(x => x.FromId == accountId || x.ToId == accountId)
                .Where(x => x.Date > transaction.Date || x.Date == transaction.Date && x.Id > transaction.Id)
                .OrderBy(x => x.Date).ThenBy(x => x.Id);
        }

        private IQueryable<Transaction> GetTransactionBeforeTransaction(int accountId, Transaction transaction)
        {
            return _dataContext.Transactions
                .Where(x => x.FromId == accountId || x.ToId == accountId)
                .Where(x => x.Date < transaction.Date || x.Date == transaction.Date && x.Id < transaction.Id)
                .OrderByDescending(x => x.Date).ThenByDescending(x => x.Id);

        }

        private static void SetBalanceForward(IQueryable<Transaction> transactions, int accountId, decimal initialBalance)
        {
            foreach (var transaction in transactions)
            {
                if (transaction.FromId == accountId)
                    initialBalance = initialBalance - transaction.Amount;
                else if (transaction.ToId == accountId)
                    initialBalance = initialBalance + transaction.Amount;
                transaction.Balance = initialBalance;
            }
        }

        private static void SetBalanceBackwards(IQueryable<Transaction> transactions, int accountId, decimal initialBalance)
        {
            foreach (var transaction in transactions)
            {
                transaction.Balance = initialBalance;
                if (transaction.FromId == accountId)
                    initialBalance = initialBalance + transaction.Amount;
                else if (transaction.ToId == accountId)
                    initialBalance = initialBalance - transaction.Amount;
            }
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
