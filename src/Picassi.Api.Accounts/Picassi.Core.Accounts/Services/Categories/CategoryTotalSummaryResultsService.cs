using System.Linq;
using Picassi.Core.Accounts.Enums;
using Picassi.Core.Accounts.Services.Transactions;
using Picassi.Core.Accounts.ViewModels.Categories;
using Picassi.Data.Accounts.Database;

namespace Picassi.Core.Accounts.Services.Categories
{
    public class CategoryTotalSummaryResultsService : ICategorySummaryService
    {
        private readonly IAccountsDataContext _dataContext;

        public CategorySummaryReportType Type => CategorySummaryReportType.Total;

        public CategoryTotalSummaryResultsService(IAccountsDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public CategorySummaryViewModel GetResults(CategorySummaryQueryModel query)
        {
            var transactions = query.Category == null
                ? _dataContext.Transactions.Where(transaction => transaction.CategoryId == null)
                : _dataContext.Transactions.Where(transaction => transaction.CategoryId == query.Category.Id);
            var results = TransactionUtils.GetTransactionsForAccountsAndDateRange(transactions, query.AccountIds, query.DateRange);

            return new CategorySummaryViewModel
            {
                Id = query.Category?.Id,
                Name = query.Category?.Name,
                TransactionCount = results.Count,
                Spend = results.Count > 0 ? results.Total : (decimal?)null
            };
        }
    }
}
