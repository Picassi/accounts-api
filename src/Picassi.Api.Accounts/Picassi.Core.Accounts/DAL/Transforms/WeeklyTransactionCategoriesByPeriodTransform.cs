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
    }
}