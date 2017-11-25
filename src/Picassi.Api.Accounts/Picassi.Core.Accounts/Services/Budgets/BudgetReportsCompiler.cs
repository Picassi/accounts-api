using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Picassi.Api.Accounts.Contract.Enums;
using Picassi.Core.Accounts.DAL;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Models.Budgets;
using Picassi.Core.Accounts.Time;

namespace Picassi.Core.Accounts.Services.Budgets
{
    public interface IBudgetReportsCompiler
    {
        IEnumerable<BudgetSummary> GetBudgetReports(BudgetsQueryModel query);
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

        public IEnumerable<BudgetSummary> GetBudgetReports(BudgetsQueryModel query)
        {
            var from = query?.DateFrom ?? DateTime.Today.AddDays(-30);
            var to = query?.DateTo ?? DateTime.Today;

            var categories = GetCategories(query);
            var transactions = GetTransactionGroups(query, @from, to);

            return MapToBudgets(categories, transactions, @from, to);
        }

        private IEnumerable<Category> GetCategories(BudgetsQueryModel query)
        {
            var categoriesQuery = _databaseProvider.GetDataContext()
                .Categories
                .Where(x => x.ParentId == null)
                .Include("Budget")
                .Include("Parent")
                .Include("Children")
                .AsQueryable();

            if (query?.Categories != null && query.Categories.Count > 0)
            {
                categoriesQuery = categoriesQuery.Where(x => query.Categories.Contains(x.Id));
            }

            if (query?.IncludeUnset != true)
            {
                categoriesQuery = categoriesQuery.Where(x => x.Budget.Any());
            }

            if (query?.CategoryType != null)
            {
                categoriesQuery = categoriesQuery.Where(x => x.CategoryType == query.CategoryType);
            }

            var categories = categoriesQuery.ToList();
            return categories;
        }

        private List<IGrouping<int?, Transaction>> GetTransactionGroups(BudgetsQueryModel query, DateTime @from, DateTime to)
        {
            var transactionsQuery = _databaseProvider.GetDataContext().Transactions
                .Where(x => x.Date >= @from && x.Date <= to);

            if (query?.Accounts != null && query.Accounts.Count > 0)
            {
                transactionsQuery = transactionsQuery.Where(x => query.Accounts.Contains(x.AccountId));
            }

            var transactions = transactionsQuery
                .GroupBy(x => x.Category.ParentId ?? x.CategoryId)
                .ToList();
            return transactions;
        }

        private IEnumerable<BudgetSummary> MapToBudgets(IEnumerable<Category> categories, List<IGrouping<int?, Transaction>> transactions, DateTime @from, DateTime to)
        {
            foreach (var c in categories)
            {
                var transactionsForCategory = transactions
                    .FirstOrDefault(t => t.Key == c.Id)?
                    .Select(t => t).ToList() ?? new List<Transaction>();

                if (c.Budget.Any())
                {
                    foreach (var b in c.Budget)
                    {
                        yield return MapToBudgetSummary(c, b, transactionsForCategory, @from, to);
                    }
                }
                else
                {
                    yield return MapToBudgetSummary(c, null, transactionsForCategory, @from, to);
                }
            }
        }

        private BudgetSummary MapToBudgetSummary(Category category, Budget budget, IEnumerable<Transaction> transactions, DateTime from, DateTime to)
        {
            var total = transactions.Sum(x => x.Amount);

            return new BudgetSummary
            {
                BudgetId = budget?.Id,
                CategoryId = category.Id,
                CategoryName = category.Name,
                Budget = budget == null ? null : (decimal?)GetBudgetAmount(budget, from, to),
                Spent = total,
                BudgetAggregationPeriod = budget?.AggregationPeriod,
                BudgetAmount = budget?.Amount,
                BudgetPeriod = budget?.Period,
                WeeklyAverage = GetAverageForPeriod(PeriodType.Week, total, from, to),
                MonthlyAverage = GetAverageForPeriod(PeriodType.Month, total, from, to)
            };
        }

        private decimal GetBudgetAmount(Budget budget, DateTime from, DateTime to)
        {
            var numberOfPeriods = _periodCalculator.GetNumberOfPeriods(
                new PeriodDefinition(budget.Period, budget.AggregationPeriod),
                new DateRange(from, to));
            return decimal.Round(budget.Amount * numberOfPeriods, 2, MidpointRounding.AwayFromZero);
        }

        private decimal GetAverageForPeriod(PeriodType type, decimal total, DateTime @from, DateTime to)
        {
            var numberOfPeriods = _periodCalculator.GetNumberOfPeriods(new PeriodDefinition(type, 1), new DateRange(from, to));
            return decimal.Round(total / numberOfPeriods, 2, MidpointRounding.AwayFromZero);
        }

    }
}
