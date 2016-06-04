using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.Enums;

namespace Picassi.Core.Accounts.Services.Reports
{
    public interface IReportResultsServiceProvider
    {
        IReportResultsService GetService(CategorySummaryReportType type);
    }

    public class ReportResultsServiceProvider : IReportResultsServiceProvider
    {
        private readonly IEnumerable<IReportResultsService> _services;

        public ReportResultsServiceProvider(IEnumerable<IReportResultsService> services)
        {
            _services = services;
        }

        public IReportResultsService GetService(CategorySummaryReportType type)
        {
            return _services.First(x => x.Type == type);
        }
    }
}
