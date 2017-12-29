using System;
using System.Data.Entity;
using System.Linq;
using Picassi.Api.Accounts.Contract.Calendar;
using Picassi.Core.Accounts.DAL.Services;

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
    }
}