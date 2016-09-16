using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.ViewModels.Reports;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.Services.Reports
{
    public interface IReportResultsViewModelFactory
    {
        ReportResultsViewModel BuildViewModel(int reportId, IEnumerable<ReportGroup> reportGroups,
            IEnumerable<TransactionCategoryGroupingViewModel> transactionSummaries);
    }

    public class ReportResultsViewModelFactory : IReportResultsViewModelFactory
    {
        public ReportResultsViewModel BuildViewModel(int reportId, IEnumerable<ReportGroup> reportGroups, IEnumerable<TransactionCategoryGroupingViewModel> transactionSummaries)
        {
            return new ReportResultsViewModel
            {
                Id = reportId,
                ReportGroups = reportGroups
                    .Select(group => new ReportResultsGroupViewModel
                    {
                        Title = @group.Name,
                        ReportLines = @group.Categories
                            .OrderBy(category => category.Ordinal)
                            .Select(category => transactionSummaries.FirstOrDefault(x => x.CategoryId == category.CategoryId) 
                                ?? new TransactionCategoryGroupingViewModel
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
