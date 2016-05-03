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
        public List<ReportResultsLineViewModel> ReportLines { get; set; }
    }

    public class ReportResultsLineViewModel
    {
        public string Name { get; set; }

        public decimal Amount { get; set; }
    }
}
