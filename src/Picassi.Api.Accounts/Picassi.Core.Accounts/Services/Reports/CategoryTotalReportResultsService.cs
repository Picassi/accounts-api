using Picassi.Core.Accounts.Enums;
using Picassi.Core.Accounts.ViewModels.Reports;

namespace Picassi.Core.Accounts.Services.Reports
{
    public class CategoryTotalReportResultsService : IReportResultsService
    {
        private readonly IReportGroupsProvider _reportGroupsProvider;
        private readonly ITransactionTotalSummariesProvider _transactionTotalSummariesProvider;
        private readonly IReportResultsViewModelFactory _reportResultsViewModelFactory;

        public CategoryTotalReportResultsService(
            IReportGroupsProvider reportGroupsProvider, 
            ITransactionTotalSummariesProvider transactionTotalSummariesProvider, 
            IReportResultsViewModelFactory reportResultsViewModelFactory)
        {
            _reportGroupsProvider = reportGroupsProvider;
            _transactionTotalSummariesProvider = transactionTotalSummariesProvider;
            _reportResultsViewModelFactory = reportResultsViewModelFactory;
        }

        public CategorySummaryReportType Type => CategorySummaryReportType.Total;


        public ReportResultsViewModel GetResults(int id, ReportResultsQueryModel query)
        {
            var reportGroups = _reportGroupsProvider.GetReportGroups(id);
            var transactionSummaries = _transactionTotalSummariesProvider.GetTransactionSummaries(id, query);
            return _reportResultsViewModelFactory.BuildViewModel(id, reportGroups, transactionSummaries);
        }
    }
}
