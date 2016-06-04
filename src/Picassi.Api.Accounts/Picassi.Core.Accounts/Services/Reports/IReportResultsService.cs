using Picassi.Core.Accounts.Enums;
using Picassi.Core.Accounts.ViewModels.Reports;

namespace Picassi.Core.Accounts.Services.Reports
{
    public interface IReportResultsService
    {
        CategorySummaryReportType Type { get; }

        ReportResultsViewModel GetResults(int id, ReportResultsQueryModel query);
    }
}
