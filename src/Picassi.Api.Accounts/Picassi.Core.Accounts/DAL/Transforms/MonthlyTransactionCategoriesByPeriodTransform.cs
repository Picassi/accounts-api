using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Picassi.Api.Accounts.Contract.Calendar;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.ModelledTransactions;

namespace Picassi.Core.Accounts.DAL.Transforms
{
    public class MonthlyTransactionCategoriesByPeriodTransform : AbstractTransactionCategoriesByPeriodTransform
    {
        public MonthlyTransactionCategoriesByPeriodTransform() : base(ReportingPeriod.Month)
        {
        }

        protected override IQueryable<IGrouping<int, ProjectedTransaction>> GroupTransactions(IQueryable<ProjectedTransaction> queryResults, DateTime baseDate)
        {
            return queryResults.GroupBy(transaction => (int)DbFunctions.DiffMonths(baseDate, transaction.Date));
        }

        protected override string GetDescription(int key, DateTime baseDate)
        {
            return baseDate.AddMonths(key).ToString("M");
        }

        protected override DateTime GetStartDate(int key, DateTime baseDate)
        {
            var date = baseDate.AddMonths(key);
            return new DateTime(date.Year, date.Month, 1);
        }

        protected override List<TransactionCategoriesGroupedByPeriodModel> AddDefaults(
            IList<GroupedTransactions> results, DateTime start, DateTime end)
        {
            var resultsToReturn = new List<TransactionCategoriesGroupedByPeriodModel>();
            var month = 0;
            for (var date = start; date < end; date = date.AddMonths(1))
            {
                var groupedModel = BuildTransactionCategoryModel(results.FirstOrDefault(r => r.Key == month), month, date);
                resultsToReturn.Add(groupedModel);
                month++;
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