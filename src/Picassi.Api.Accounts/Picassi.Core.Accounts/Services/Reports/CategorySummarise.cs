using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Api.Accounts.Contract.Transactions;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.Categories;
using Picassi.Core.Accounts.Models.Transactions;

namespace Picassi.Core.Accounts.Services.Reports
{
    public interface ICategorySummariser
    {
        CategorySummaryViewModel GetCategorySummary(int categoryId, DateTime? from, DateTime? to);

        IEnumerable<CategorySummaryViewModel> GetCategorySummaries(DateTime? from, DateTime? to);
        CategorySpendingSummaryViewModel GetCategorySpendingSummary(int id, DateTime? queryDateFrom, DateTime? queryDateTo);
    }

    public class CategorySummariser : ICategorySummariser
    {
        private readonly ITransactionsDataService _transactionsDataService;
        private readonly ICategoriesDataService _categoriesDataService;

        public CategorySummariser(ITransactionsDataService transactionsDataService, ICategoriesDataService categoriesDataService)
        {
            _transactionsDataService = transactionsDataService;
            _categoriesDataService = categoriesDataService;
        }

        public CategorySummaryViewModel GetCategorySummary(int categoryId, DateTime? from, DateTime? to)
        {
            var categoryIds = new [] { categoryId };
            var transactions = _transactionsDataService.Query(categories: categoryIds, dateFrom: from, dateTo: to).ToList();
            return GetCategorySummaryModel(categoryId, from, to, transactions);
        }

        public IEnumerable<CategorySummaryViewModel> GetCategorySummaries(DateTime? @from, DateTime? to)
        {
            var transactions = _transactionsDataService.Query(dateFrom: from, dateTo: to).ToList();
            var categories = _categoriesDataService.Query(new CategoriesQueryModel());
            return categories.Select(c => GetCategorySummaryModel(c.Id, from, to, transactions.Where(t => t.CategoryId == c.Id).ToList()));
        }

        public CategorySpendingSummaryViewModel GetCategorySpendingSummary(int id, DateTime? from, DateTime? to)
        {
            var categoryIds = new[] { id };
            var transactions = _transactionsDataService
                .Query(categories: categoryIds, dateFrom: from, dateTo: to, showUncategorised: false)
                .OrderBy(t => t.Date)
                .ToList();

            var dateFrom = from ?? (transactions.Any() ? transactions.First().Date : DateTime.Today.AddMonths(-1));
            var dateTo = to ?? (transactions.Any() ? transactions.Last().Date : DateTime.Today);

            return GetCategorySpendingSummaryModel(id, dateFrom, dateTo, transactions);
        }

        private static CategorySummaryViewModel GetCategorySummaryModel(int? categoryId, DateTime? from, DateTime? to, IList<TransactionModel> transactions)
        {
            var credit = transactions.Where(x => x.Amount > 0).Select(x => x.Amount).DefaultIfEmpty(0).Sum();
            var debit = transactions.Where(x => x.Amount < 0).Select(x => x.Amount).DefaultIfEmpty(0).Sum();

            return new CategorySummaryViewModel
            {
                CategoryId = categoryId,
                From = from,
                To = to,
                Budget = 0,
                TotalDebit = debit,
                TotalCredit = credit,
                TotalChange = credit - debit
            };
        }

        private CategorySpendingSummaryViewModel GetCategorySpendingSummaryModel(int id, DateTime from, DateTime to, IEnumerable<TransactionModel> transactions)
        {
            var total = transactions.Select(x => x.Amount).DefaultIfEmpty(0).Sum();
            var period = (to - from).TotalDays;
            var weeks = (decimal) (period / 7);
            var months = (decimal) (period * 12 / 365.25);

            return new CategorySpendingSummaryViewModel
            {
                CategoryId = id,
                From = from,
                To = to,
                Total = total,
                WeeklyAverage = Math.Round(total / weeks, 2),
                MonthlyAverage = Math.Round(total / months, 2)
            };
        }

    }
}
