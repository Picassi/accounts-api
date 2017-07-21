using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Models.Budgets;
using Picassi.Core.Accounts.Time;

namespace Picassi.Core.Accounts.Services.Budgets
{
    public interface IBudgetReportsCompiler
    {
        IEnumerable<BudgetSummary> GetBudgetReports(DateTime from, DateTime to);
    }
    public class BudgetReportsCompiler : IBudgetReportsCompiler
    {
        private readonly IAccountsDatabaseProvider _databaseProvider;
        private readonly IPeriodCalculator _periodCalculator;

        public BudgetReportsCompiler(IAccountsDatabaseProvider databaseProvider, IPeriodCalculator periodCalculator)
        {
            _databaseProvider = databaseProvider;
            _periodCalculator = periodCalculator;
        }

        public IEnumerable<BudgetSummary> GetBudgetReports(DateTime from, DateTime to)
        {
            var categories = _databaseProvider.GetDataContext().Categories.Include("Budget").ToList();
            var transactions = _databaseProvider.GetDataContext().Transactions
                .Where(x => x.Date >= from && x.Date <= to)
                .GroupBy(x => x.CategoryId)
                .ToList();

            foreach (var c in categories)
            {
                var transactionsForCategory = transactions.FirstOrDefault(t => t.Key == c.Id)
                    ?.Select(t => t).ToList() ?? new List<Transaction>();
                if (c.Budget.Any())
                {
                    foreach (var b in c.Budget)
                    {
                        yield return MapToBudgetSummary(c, b, transactionsForCategory, from, to);
                    }
                }
                else
                {
                    yield return MapToBudgetSummary(c, null, transactionsForCategory, from, to);
                }
            }
        }

        private BudgetSummary MapToBudgetSummary(Category category, Budget budget, IEnumerable<Transaction> transactions, DateTime from, DateTime to)
        {
            return new BudgetSummary
            {
                CategoryId = category.Id,
                CategoryName = category.Name,
                Budget = budget == null ? null : (decimal?)GetBudgetAmount(budget, from, to),
                Spent = transactions.Sum(x => x.Amount)
            };
        }

        private decimal GetBudgetAmount(Budget budget, DateTime from, DateTime to)
        {
            var numberOfPeriods = _periodCalculator.GetNumberOfPeriods(
                new PeriodDefinition(budget.Period, budget.AggregationPeriod),
                new DateRange(from, to));
            return budget.Amount * numberOfPeriods;
        }
    }
}
