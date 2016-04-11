using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DbAccess.Categories;
using Picassi.Core.Accounts.Time;
using Picassi.Core.Accounts.Time.Periods;
using Picassi.Core.Accounts.ViewModels.Categories;

namespace Picassi.Core.Accounts.Reports
{
    public interface ICategorySummaryService
    {
        IEnumerable<CategorySummaryViewModel> GetCategorySummaries(CategoriesQueryModel query);
    }

    public class CategorySummaryService : ICategorySummaryService
    {
        private readonly ICategorySummaryViewModelFactory _categorySummaryViewModelFactory;
        private readonly ICategoryQueryService _categoryQueryService;

        public CategorySummaryService(ICategorySummaryViewModelFactory categorySummaryViewModelFactory, ICategoryQueryService categoryQueryService)
        {
            _categorySummaryViewModelFactory = categorySummaryViewModelFactory;
            _categoryQueryService = categoryQueryService;
        }

        public IEnumerable<CategorySummaryViewModel> GetCategorySummaries(CategoriesQueryModel query)
        {
            var categories = _categoryQueryService.Query(query);
            var categoriesPlusUncategorised = AddPlaceholderForUncategorised(categories);
            var dateRange = query.DateFrom != null && query.DateTo != null
                ? new DateRange((DateTime) query.DateFrom, (DateTime) query.DateTo)
                : null;
            return BuildCategorySummaries(categoriesPlusUncategorised, query.AccountIds, query.Frequency, dateRange);
        }

        private IEnumerable<CategorySummaryViewModel> BuildCategorySummaries(IEnumerable<CategoryViewModel> categories, int[] referenceAccountIds, PeriodType periodType, DateRange range)
        {
            return categories.Select(x => _categorySummaryViewModelFactory.GetSummary(new CategorySummaryQueryModel
            {
                AccountIds = referenceAccountIds.ToList(),
                AverageSpendPeriod = new PeriodDefinition { Quantity = 1, Type =periodType },
                Category = x,
                DateRange = range
            }));
        }

        private static IEnumerable<CategoryViewModel> AddPlaceholderForUncategorised(IEnumerable<CategoryViewModel> categories)
        {
            return new List<CategoryViewModel> { null }.Union(categories).ToList();
        }
    }
}
