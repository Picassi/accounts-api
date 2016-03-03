using System.Collections.Generic;
using System.Linq;
using Picassi.Common.Data.Enums;
using Picassi.Core.Accounts.DbAccess.Categories;
using Picassi.Core.Accounts.ViewModels.Categories;

namespace Picassi.Core.Accounts.Reports
{
    public interface ICategorySummaryService
    {
        IEnumerable<CategorySummaryViewModel> GetCategorySummaries(CategoriesQueryModel query, Frequency frequency, int periods);
    }

    public class CategorySummaryViewModelFactory : ICategorySummaryService
    {
        private readonly ICategorySummaryViewModelFactory _categorySummaryViewModelFactory;
        private readonly ICategoryQueryService _categoryQueryService;

        public CategorySummaryViewModelFactory(ICategorySummaryViewModelFactory categorySummaryViewModelFactory, ICategoryQueryService categoryQueryService)
        {
            _categorySummaryViewModelFactory = categorySummaryViewModelFactory;
            _categoryQueryService = categoryQueryService;
        }

        public IEnumerable<CategorySummaryViewModel> GetCategorySummaries(CategoriesQueryModel query, Frequency frequency, int periods)
        {
            var categories = _categoryQueryService.Query(query);
            var categoriesPlusUncategorised = AddPlaceholderForUncategorised(categories);
            return BuildCategorySummaries(categoriesPlusUncategorised, frequency, periods);
        }

        private IEnumerable<CategorySummaryViewModel> BuildCategorySummaries(IEnumerable<CategoryViewModel> categories, Frequency frequency, int periods)
        {
            return categories.Select(x => _categorySummaryViewModelFactory.GetSummary(x, frequency, periods));
        }

        private static IEnumerable<CategoryViewModel> AddPlaceholderForUncategorised(IEnumerable<CategoryViewModel> categories)
        {
            return new List<CategoryViewModel> { null }.Union(categories).ToList();
        }
    }
}
