using System.Collections.Generic;
using System.Linq;

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

        public decimal Total { get { return ReportLines.Sum(x => x.Amount);  } }

        public IEnumerable<TransactionCategoryGroupingViewModel> ReportLines { get; set; }
    }

    public class TransactionCategoryGroupingViewModel
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }        
    }
}
