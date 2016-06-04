using Picassi.Core.Accounts.Enums;
using Picassi.Core.Accounts.ViewModels.Reports;

namespace Picassi.Core.Accounts.Services.Reports
{
    public class CategoryAverageReportResultsService : IReportResultsService
    {
        private readonly IReportGroupsProvider _reportGroupsProvider;
        private readonly ITransactionAverageSummariesProvider _transactionAverageSummariesProvider;
        private readonly IReportResultsViewModelFactory _reportResultsViewModelFactory;

        public CategoryAverageReportResultsService(
            IReportGroupsProvider reportGroupsProvider,
            ITransactionAverageSummariesProvider transactionAverageSummariesProvider,
            IReportResultsViewModelFactory reportResultsViewModelFactory)
        {
            _reportGroupsProvider = reportGroupsProvider;
            _transactionAverageSummariesProvider = transactionAverageSummariesProvider;
            _reportResultsViewModelFactory = reportResultsViewModelFactory;
        }

        public CategorySummaryReportType Type => CategorySummaryReportType.Average;


        public ReportResultsViewModel GetResults(int id, ReportResultsQueryModel query)
        {
            var reportGroups = _reportGroupsProvider.GetReportGroups(id);
            var transactionSummaries = _transactionAverageSummariesProvider.GetTransactionSummaries(id, query);
            return _reportResultsViewModelFactory.BuildViewModel(id, reportGroups, transactionSummaries);
        }
    }
}
