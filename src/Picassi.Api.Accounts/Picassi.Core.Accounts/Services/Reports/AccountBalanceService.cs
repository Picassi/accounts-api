using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL;
using Picassi.Core.Accounts.DAL.Entities;

namespace Picassi.Core.Accounts.Services.Reports
{
    public interface IAccountBalanceService
    {
        decimal GetAccountBalance(int accountId, DateTime date);
        void SetTransactionBalances(int accountId, DateTime date);
        void SetTransactionBalances(int accountId, DateTime date, Transaction transaction, decimal initialBalance, decimal balanceAfter);
        
    }

    public class AccountBalanceService : IAccountBalanceService
    {
        private readonly IAccountsDataContext _dataContext;

        public AccountBalanceService(IAccountsDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public decimal GetAccountBalance(int accountId, DateTime date)
        {
            return GetTransactionForAccountBalance(accountId, date)?.Balance ?? 0;
        }

        public void SetTransactionBalances(int accountId, DateTime date)
        {
            var transaction = GetTransactionForAccountBalance(accountId, date);
            var initialBalance = transaction?.Balance ?? 0;
            var balanceAfter = initialBalance - transaction?.Amount ?? 0;
            SetTransactionBalances(accountId, date, transaction, initialBalance, balanceAfter);
        }

        public void SetTransactionBalances(int accountId, DateTime date, Transaction transaction, decimal initialBalance, decimal balanceAfter)
        {
            if (transaction == null) return;

            SetBalanceForward(GetTransactionsAfterTransaction(accountId, transaction).ToList(), balanceAfter);
            SetBalanceBackwards(GetTransactionBeforeTransaction(accountId, transaction).ToList(), initialBalance);

            _dataContext.SaveChanges();
        }

        private IQueryable<Transaction> GetTransactionsAfterTransaction(int accountId, Transaction transaction)
        {
            return _dataContext.Transactions                
                .Where(x => x.AccountId == accountId)
                .Where(x => x.Date > transaction.Date || (x.Date == transaction.Date && x.Ordinal > transaction.Ordinal))
                .OrderBy(x => x.Date).ThenBy(x => x.Ordinal);
        }

        private IQueryable<Transaction> GetTransactionBeforeTransaction(int accountId, Transaction transaction)
        {
            return _dataContext.Transactions
                .Where(x => x.AccountId == accountId)
                .Where(x => x.Date < transaction.Date || (x.Date == transaction.Date && x.Ordinal < transaction.Ordinal))
                .OrderByDescending(x => x.Date).ThenByDescending(x => x.Ordinal);

        }

        private static void SetBalanceForward(IEnumerable<Transaction> transactions, decimal initialBalance)
        {
            foreach (var transaction in transactions)
            {
                initialBalance = initialBalance + transaction.Amount;
                transaction.Balance = initialBalance;
            }
        }

        private static void SetBalanceBackwards(IEnumerable<Transaction> transactions, decimal initialBalance)
        {
            foreach (var transaction in transactions)
            {
                transaction.Balance = initialBalance;
                initialBalance = initialBalance - transaction.Amount;
            }
        }

        private Transaction GetTransactionForAccountBalance(int accountId, DateTime date)
        {
            var lastTransaction = _dataContext
                .Transactions.Where(x => x.AccountId == accountId && x.Date < date)
                .OrderByDescending(transaction => transaction.Date)
                .ThenByDescending(transaction => transaction.Ordinal)
                .FirstOrDefault();
            var firstTransactionToday = _dataContext
                .Transactions.Where(x => x.AccountId == accountId && x.Date == date)
                .OrderBy(transaction => transaction.Ordinal)
                .FirstOrDefault();
            return (lastTransaction ?? firstTransactionToday);
        }
    }
}
