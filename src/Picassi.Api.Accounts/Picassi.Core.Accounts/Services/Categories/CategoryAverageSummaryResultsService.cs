using System.Linq;
using Picassi.Core.Accounts.Enums;
using Picassi.Core.Accounts.Services.Transactions;
using Picassi.Core.Accounts.Time;
using Picassi.Core.Accounts.ViewModels.Categories;
using Picassi.Data.Accounts.Database;

namespace Picassi.Core.Accounts.Services.Categories
{
    public class CategoryAverageSummaryResultsService : ICategorySummaryService
    {
        private readonly IAccountsDataContext _dataContext;
        private readonly IPeriodCalculator _periodCalculator;

        public CategorySummaryReportType Type => CategorySummaryReportType.Average;

        public CategoryAverageSummaryResultsService(IAccountsDataContext dataContext, IPeriodCalculator periodCalculator)
        {
            _dataContext = dataContext;
            _periodCalculator = periodCalculator;
        }

        public CategorySummaryViewModel GetResults(CategorySummaryQueryModel query)
        {
            var transactions = query.Category == null 
                ? _dataContext.Transactions.Where(transaction => transaction.CategoryId == null) 
                : _dataContext.Transactions.Where(transaction => transaction.CategoryId == query.Category.Id);
            var results = TransactionUtils.GetTransactionsForAccountsAndDateRange(transactions, query.AccountIds, query.DateRange);
            var periods = _periodCalculator.GetNumberOfPeriods(query.AverageSpendPeriod, query.DateRange);

            return new CategorySummaryViewModel
            {
                Id = query.Category?.Id,
                Name = query.Category?.Name,
                TransactionCount = results.Count / periods,
                Spend = results.Total / periods
            };
        }
    }
}