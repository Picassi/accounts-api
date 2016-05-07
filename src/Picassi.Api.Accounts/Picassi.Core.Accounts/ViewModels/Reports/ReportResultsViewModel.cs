using System.Collections.Generic;

namespace Picassi.Core.Accounts.ViewModels.Reports
{
    public class ReportResultsViewModel
    {
        public int Id { get; set; }

        public List<ReportResultsGroupViewModel> ReportGroups { get; set; }
    }

    public class ReportResultsGroupViewModel
    {
        public string Title { get; set; }

        public List<ReportResultsLineViewModel> ReportLines { get; set; }
    }

    public class ReportResultsLineViewModel
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }        
    }
}
