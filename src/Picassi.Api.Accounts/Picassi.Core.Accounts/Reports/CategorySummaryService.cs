using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DbAccess.Categories;
using Picassi.Core.Accounts.Enums;
using Picassi.Core.Accounts.Services.Categories;
using Picassi.Core.Accounts.Time;
using Picassi.Core.Accounts.Time.Periods;
using Picassi.Core.Accounts.ViewModels.Categories;

namespace Picassi.Core.Accounts.Reports
{
    public interface ICategorySummaryService
    {
        CategorySummaryResultsViewModel GetCategorySummaries(CategoriesQueryModel query);
    }

    public class CategorySummaryService : ICategorySummaryService
    {
        private readonly ICategoryQueryService _categoryQueryService;
        private readonly ICategorySummaryServiceProvider _summaryServiceProvider;

        public CategorySummaryService(ICategoryQueryService categoryQueryService, ICategorySummaryServiceProvider summaryServiceProvider)
        {
            _categoryQueryService = categoryQueryService;
            _summaryServiceProvider = summaryServiceProvider;
        }

        public CategorySummaryResultsViewModel GetCategorySummaries(CategoriesQueryModel query)
        {
            var lines = GetCategorySummaryResults(query);
            return new CategorySummaryResultsViewModel
            {
                TotalLines = lines.Count,
                Total = lines.Where(x => x.Spend != null).Sum(x => (decimal)x.Spend),
                TotalTransactions = lines.Sum(x => x.TransactionCount),
                Lines = lines
            };
        }

        private List<CategorySummaryViewModel> GetCategorySummaryResults(CategoriesQueryModel query)
        {
            var service = _summaryServiceProvider.GetService(query.ReportType);
            var categoriesPlusUncategorised = GetCategories(query);
            var dateRange = query.DateFrom != null && query.DateTo != null
                ? new DateRange((DateTime) query.DateFrom, (DateTime) query.DateTo)
                : null;
            var lines = categoriesPlusUncategorised.Select(category =>
                service.GetResults(BuildQueryModel(category, query.AccountIds, query.ReportType, query.Frequency, dateRange)))
                .ToList();
            return lines;
        }

        private IEnumerable<CategoryViewModel> GetCategories(CategoriesQueryModel query)
        {
            var categories = _categoryQueryService.Query(query);
            return categories.Union(new List<CategoryViewModel> { null }).ToList();
        }

        private CategorySummaryQueryModel BuildQueryModel(
            CategoryViewModel category,
            IEnumerable<int> referenceAccountIds, 
            CategorySummaryReportType reportType,
            PeriodType periodType, 
            DateRange range)
        {
            return new CategorySummaryQueryModel
            {
                AccountIds = referenceAccountIds?.ToList() ?? new List<int>(),
                ReportType = reportType,
                AverageSpendPeriod = new PeriodDefinition {Quantity = 1, Type = periodType},
                Category = category,
                DateRange = range
            };
        }
    }
}
