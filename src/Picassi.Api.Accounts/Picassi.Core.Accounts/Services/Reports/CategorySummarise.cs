using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.Categories;
using Picassi.Core.Accounts.Models.Transactions;

namespace Picassi.Core.Accounts.Services.Reports
{
    public interface ICategorySummariser
    {
        CategorySummaryViewModel GetCategorySummary(int categoryId, DateTime? from, DateTime? to);

        IEnumerable<CategorySummaryViewModel> GetCategorySummaries(DateTime? from, DateTime? to);
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
    }
}
