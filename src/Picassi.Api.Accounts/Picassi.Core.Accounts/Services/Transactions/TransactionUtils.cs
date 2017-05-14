using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Models.Transactions;
using Picassi.Core.Accounts.Time;

namespace Picassi.Core.Accounts.Services.Transactions
{
    public static class TransactionUtils
    {
        public static TransactionSummaryModel GetTransactionsForAccountsAndDateRange(IQueryable<Transaction> transactions, ICollection<int> accountIds, DateRange range)
        {
            if (range != null)
            {
                transactions = transactions.Where(transaction => transaction.Date >= range.Start && transaction.Date < range.End);
            }
            return new TransactionSummaryModel
            {
                Count = GetTransactionCount(transactions, accountIds),
                Total = GetTransactionTotal(transactions, accountIds)
            };
        }

        public static decimal GetTransactionCount(IQueryable<Transaction> transactions, ICollection<int> accounts = null)
        {
            return GetTransactionsForAccounts(transactions, accounts).Count();
        }

        public static decimal GetTransactionTotal(IQueryable<Transaction> transactions, ICollection<int> accounts = null)
        {
            return GetTransactionsForAccounts(transactions, accounts)
                    .Select(x => x.Amount)
                    .DefaultIfEmpty(0)
                    .Sum();
        }

        public static IQueryable<Transaction> GetTransactionsForAccounts(IQueryable<Transaction> transactions, ICollection<int> accounts = null)
        {
            return accounts == null ? transactions : transactions.Where(x => accounts.Contains((int) x.AccountId));
        }
    }
}
