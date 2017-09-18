using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Time;
using Picassi.Core.Accounts.Time.Periods;

namespace Picassi.Core.Accounts.Services.Projections.BudgetProjectors
{
    public class WeeklyBudgetProjector : IBudgetProjector
    {
        public PeriodType Type => PeriodType.Week;

        public IEnumerable<ModelledTransaction> GetProjectionsForBudget(Budget budget, DateTime start, DateTime end, IList<ModelledTransaction> existingTransactions)
        {
            if (start > end) return new List<ModelledTransaction>();
            var transactions = DateUtilities.GetAllMatchingDaysOfTheWeek(start, end, DayOfWeek.Monday)
                .Select(date => BuildTransaction(budget, date, GetExistingAmountForCategory(date, budget.CategoryId, existingTransactions))).ToList();

            return transactions;
        }

        private static decimal GetExistingAmountForCategory(DateTime date, int categoryId, IList<ModelledTransaction> existingTransactions)
        {
            return existingTransactions
                .Where(x => x.Date >= date && x.Date < date.AddDays(7) && x.CategoryId == categoryId)
                .Select(c => c.Amount).DefaultIfEmpty(0).Sum();
        }


        private static ModelledTransaction BuildTransaction(Budget weeklyBudget, DateTime date, decimal existingAmount)
        {
            return new ModelledTransaction
            {
                Amount = -weeklyBudget.Amount - existingAmount,
                Date = date,
                Description = "Budget",
                BudgetId = weeklyBudget.Id,
                AccountId = weeklyBudget.AccountId,
                CategoryId = weeklyBudget.CategoryId
            };
        }
    }
}
