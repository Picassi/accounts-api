using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using Picassi.Core.Accounts.DAL;
using Picassi.Core.Accounts.Models.Categories;

namespace Picassi.Core.Accounts.Services.Reports
{
    public interface ICategorySummaryService
    {
        CategorySummaryResultsViewModel GetCategorySummaries(CategoriesQueryModel query);
    }

    public class CategorySummaryService : ICategorySummaryService
    {
        private readonly IAccountsDataContext _dataContext;

        public CategorySummaryService(IAccountsDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public CategorySummaryResultsViewModel GetCategorySummaries(CategoriesQueryModel query)
        {
            var results = GetCategoryByAccountSummaries(query);
            var grouped = GroupByFilteredAccounts(query, results);
            var lines = CompileSummaries(grouped);
            return new CategorySummaryResultsViewModel(lines.ToList());
        }

        private DbRawSqlQuery<CategorySummaryResult> GetCategoryByAccountSummaries(CategoriesQueryModel query)
        {
            var dateFrom = query.DateFrom ?? DateTime.UtcNow.AddMonths(-1);
            var dateTo = query.DateTo ?? DateTime.UtcNow;
            var startDate = new SqlParameter("@StartDate", dateFrom);
            var endDate = new SqlParameter("@EndDate", dateTo);
            return _dataContext.Query<CategorySummaryResult>(
                "exec accounts.GetTransactionTotals @StartDate , @EndDate", startDate, endDate);
        }

        private static IEnumerable<CategoryAccountSummaryResult> GroupByFilteredAccounts(CategoriesQueryModel query, 
            DbRawSqlQuery<CategorySummaryResult> results)
        {
            var filtered = query.AllAccounts
                ? results
                : results.Where(r => query.AccountIds != null && query.AccountIds.Contains(r.AccountId));

            var filteredList = filtered.ToList();

            var grouped = filteredList
                .GroupBy(r => new { r.CategoryId, r.CategoryName })
                .Select(r => new CategoryAccountSummaryResult(r.Key.CategoryId, r.Key.CategoryName, r.ToList()));

            return grouped;
        }

        private static IEnumerable<CategorySummaryViewModel> CompileSummaries(IEnumerable<CategoryAccountSummaryResult> grouped)
        {
            var lines = grouped.GroupBy(r => new { r.CategoryId, r.CategoryName })
                .Select(l => new CategorySummaryViewModel(l.Key.CategoryId, l.Key.CategoryName, l.ToList()));
            return lines;
        }
    }
}
