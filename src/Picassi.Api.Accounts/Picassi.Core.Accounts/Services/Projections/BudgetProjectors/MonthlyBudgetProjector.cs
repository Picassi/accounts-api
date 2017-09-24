using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Api.Accounts.Contract.Enums;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Time;
using Picassi.Core.Accounts.Time.Periods;

namespace Picassi.Core.Accounts.Services.Projections.BudgetProjectors
{
    public class MonthlyBudgetProjector : IBudgetProjector
    {
        public PeriodType Type => PeriodType.Month;

        public IEnumerable<ModelledTransaction> GetProjectionsForBudget(Budget budget, DateTime start, DateTime end, IList<ModelledTransaction> existingTransactions)
        {
            if (start > end) return new List<ModelledTransaction>();

            var transactions = DateUtilities.GetAllMatchingDaysOfTheMonth(start, end, 1)
                .Select(date => BuildTransaction(budget, date, GetExistingAmountForCategory(date, budget.CategoryId, existingTransactions))).ToList();

            return transactions;
        }

        private static decimal GetExistingAmountForCategory(DateTime date, int categoryId, IList<ModelledTransaction> existingTransactions)
        {
            return existingTransactions.Where(x => x.Date.Month == date.Month && x.CategoryId == categoryId).Select(c => c.Amount).DefaultIfEmpty(0).Sum();
        }

        private static ModelledTransaction BuildTransaction(Budget monthlyBudget, DateTime date, decimal existingAmount)
        {
            return new ModelledTransaction
            {
                Amount = -monthlyBudget.Amount - existingAmount,
                Date = date,
                Description = "Budget",
                ScheduledTransactionId = monthlyBudget.Id,
                AccountId = monthlyBudget.AccountId,
                CategoryId = monthlyBudget.CategoryId
            };
        }
    }
}
