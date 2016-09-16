using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.Time;
using Picassi.Core.Accounts.ViewModels.Reports;
using Picassi.Core.Accounts.ViewModels.Transactions;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.Services.Transactions
{
    public static class TransactionUtils
    {
        public static IEnumerable<TransactionCategoryGroupingViewModel> GroupTransactionsByCategory(IQueryable<Transaction> transactions, ICollection<int> accounts = null)
        {
            return transactions.GroupBy(transaction => new { transaction.Category.Id, transaction.Category.Name })
                .Select(group => new TransactionCategoryGroupingViewModel
                {
                    CategoryId = @group.Key.Id,
                    Name = @group.Key.Name,
                    Amount = GetTransactionTotal(transactions, accounts)
                }).ToList();
        }

        public static TransactionSummaryViewModel GetTransactionsForAccountsAndDateRange(IQueryable<Transaction> transactions, ICollection<int> accountIds, DateRange range)
        {
            if (range != null)
            {
                transactions = transactions.Where(transaction => transaction.Date >= range.Start && transaction.Date < range.End);
            }
            return new TransactionSummaryViewModel
            {
                Count = GetTransactionCount(transactions, accountIds),
                Total = GetTransactionTotal(transactions, accountIds)
            };
        }

        public static decimal GetTransactionCount(IQueryable<Transaction> transactions, ICollection<int> accounts = null)
        {
            var trans = accounts == null 
                ? transactions.Where(x => (x.ToId != null || x.FromId != null) && !(x.ToId != null && x.FromId != null))
                : transactions.Where(x => 
                    ((x.ToId != null && accounts.Contains((int)x.ToId)) || (x.FromId != null && accounts.Contains((int)x.FromId))) &&
                   !((x.ToId != null && accounts.Contains((int)x.ToId)) && (x.FromId != null && accounts.Contains((int)x.FromId))));
            return trans.Count();
        }

        public static decimal GetTransactionTotal(IQueryable<Transaction> transactions, ICollection<int> accounts = null)
        {
            var to = accounts == null
                ? transactions.Where(x => x.ToId != null).Select(x => x.Amount).DefaultIfEmpty(0).Sum()
                : transactions.Where(x => x.ToId != null && accounts.Contains((int) x.ToId))
                    .Select(x => x.Amount)
                    .DefaultIfEmpty(0)
                    .Sum();

            var from = accounts == null
                ? transactions.Where(x => x.FromId != null).Select(x => x.Amount).DefaultIfEmpty(0).Sum()
                : transactions.Where(x => x.FromId != null && accounts.Contains((int) x.FromId))
                    .Select(x => x.Amount)
                    .DefaultIfEmpty(0)
                    .Sum();

            return to - from;
        }
    }
}
