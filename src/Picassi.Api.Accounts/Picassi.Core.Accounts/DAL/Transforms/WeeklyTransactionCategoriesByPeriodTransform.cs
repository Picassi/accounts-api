using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Picassi.Api.Accounts.Contract.Calendar;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.ModelledTransactions;

namespace Picassi.Core.Accounts.DAL.Transforms
{
    public class WeeklyTransactionCategoriesByPeriodTransform : AbstractTransactionCategoriesByPeriodTransform
    {
        public WeeklyTransactionCategoriesByPeriodTransform() : base(ReportingPeriod.Week)
        {
        }

        protected override IQueryable<IGrouping<int, ProjectedTransaction>> GroupTransactions(IQueryable<ProjectedTransaction> queryResults, DateTime baseDate)
        {
            return queryResults.GroupBy(transaction => (int)DbFunctions.DiffDays(baseDate, transaction.Date) / 7);
        }

        protected override string GetDescription(int key, DateTime baseDate)
        {
            return "Week " + key;
        }

        protected override DateTime GetStartDate(int key, DateTime baseDate)
        {
            return baseDate.AddDays(key * 7);
        }

        protected override List<TransactionCategoriesGroupedByPeriodModel> AddDefaults(
            IList<GroupedTransactions> results, DateTime start, DateTime end)
        {
            var resultsToReturn = new List<TransactionCategoriesGroupedByPeriodModel>();
            var week = 0;
            for (var date = start; date <= end; date = date.AddDays(7))
            {
                week++;
                var groupedModel = BuildTransactionCategoryModel(results.FirstOrDefault(r => r.Key == week), week, date);
                resultsToReturn.Add(groupedModel);
            }
            return resultsToReturn;
        }

        private TransactionCategoriesGroupedByPeriodModel BuildTransactionCategoryModel(GroupedTransactions transactionModel, int week, DateTime start)
        {
            return new TransactionCategoriesGroupedByPeriodModel
            {
                Description = GetDescription(week, start),
                StartDate = start,
                Credit = transactionModel?.Credit ?? 0,
                Debit = transactionModel?.Debit ?? 0,
                Transactions = transactionModel?.Transactions ?? new List<TransactionsGroupedByCategoryModel>()
            };
        }
    }
}