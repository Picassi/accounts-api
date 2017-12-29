using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Api.Accounts.Contract.Calendar;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.ModelledTransactions;

namespace Picassi.Core.Accounts.DAL.Transforms
{
    public interface ITransactionCategoriesByPeriodTransform
    {
        ReportingPeriod Period { get; }

        List<TransactionCategoriesGroupedByPeriodModel> GetPeriodSummaries(
            IQueryable<ProjectedTransaction> queryResults, DateTime startTime, DateTime endDate);
    }

    public abstract class AbstractTransactionCategoriesByPeriodTransform : ITransactionCategoriesByPeriodTransform
    {
        public ReportingPeriod Period { get; }

        protected AbstractTransactionCategoriesByPeriodTransform(ReportingPeriod period)
        {
            Period = period;
        }

        public List<TransactionCategoriesGroupedByPeriodModel> GetPeriodSummaries(
            IQueryable<ProjectedTransaction> queryResults, DateTime startDate, DateTime endDate)
        {
            var groupedTransactions = GetGroupedTransactions(queryResults, startDate);
            return AddDefaults(groupedTransactions, startDate, endDate).OrderBy(x => x.StartDate).ToList();

        }

        protected abstract string GetDescription(int key, DateTime baseDate);

        protected abstract DateTime GetStartDate(int key, DateTime baseDate);

        protected abstract IQueryable<IGrouping<int, ProjectedTransaction>> GroupTransactions(
            IQueryable<ProjectedTransaction> queryResults, DateTime baseDate);

        private IList<GroupedTransactions> GetGroupedTransactions(IQueryable<ProjectedTransaction> queryResults, DateTime baseDate)
        {
            var groupedByPeriod = GroupTransactions(queryResults, baseDate);

            return groupedByPeriod.Select(grp => new GroupedTransactions
            {
                Key = grp.Key,
                Credit = grp.Where(transaction => transaction.Amount > 0)
                    .Select(transaction => transaction.Amount)
                    .DefaultIfEmpty(0).Sum(),
                Debit = grp.Where(transaction => transaction.Amount < 0)
                    .Select(transaction => transaction.Amount)
                    .DefaultIfEmpty(0).Sum(),
                Transactions = grp.GroupBy(transactions => new {transactions.CategoryId, transactions.CategoryName})
                    .Select(grp2 => new TransactionsGroupedByCategoryModel
                        {
                            Description = grp2.Key.CategoryName ?? "Uncategorised",
                            CategoryId = grp2.Key.CategoryId,
                            Count = grp2.Count(),
                            Credit = grp2.Where(transaction => transaction.Amount > 0)
                                .Select(transaction => transaction.Amount)
                                .DefaultIfEmpty(0).Sum(),
                            Debit = grp2.Where(transaction => transaction.Amount < 0)
                                .Select(transaction => transaction.Amount)
                                .DefaultIfEmpty(0).Sum()
                        }
                    )
            }).ToList();
        }

        protected abstract List<TransactionCategoriesGroupedByPeriodModel> AddDefaults(
            IList<GroupedTransactions> results, DateTime start, DateTime end);

        protected class GroupedTransactions
        {
            public IEnumerable<TransactionsGroupedByCategoryModel> Transactions { get; set; }

            public decimal Credit { get; set; }

            public decimal Debit { get; set; }

            public int Key { get; set; }
        }
    }
}
