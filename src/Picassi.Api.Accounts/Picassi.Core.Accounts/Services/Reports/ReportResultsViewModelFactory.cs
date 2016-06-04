using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.ViewModels.Reports;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.Services.Reports
{
    public interface IReportResultsViewModelFactory
    {
        ReportResultsViewModel BuildViewModel(int reportId, IEnumerable<ReportGroup> reportGroups,
            IEnumerable<ReportResultsLineViewModel> transactionSummaries);
    }

    public class ReportResultsViewModelFactory : IReportResultsViewModelFactory
    {
        public ReportResultsViewModel BuildViewModel(int reportId, IEnumerable<ReportGroup> reportGroups, IEnumerable<ReportResultsLineViewModel> transactionSummaries)
        {
            return new ReportResultsViewModel
            {
                Id = reportId,
                ReportGroups = reportGroups
                    .Select(group => new ReportResultsGroupViewModel
                    {
                        ReportLines = @group.Categories
                            .OrderBy(category => category.Ordinal)
                            .Select(category => transactionSummaries.FirstOrDefault(x => x.CategoryId == category.CategoryId) 
                                ?? new ReportResultsLineViewModel
                                {
                                    CategoryId = category.CategoryId,
                                    Name = category.Category.Name,
                                    Amount = 0
                                })
                    }).ToList()
            };
        }
    }
}
