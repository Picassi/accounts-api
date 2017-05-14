using System.Collections.Generic;
using System.Linq;

namespace Picassi.Core.Accounts.Models.Tags
{
    public class TagSummaryResultsViewModel : ResultsViewModel<TagSummaryViewModel>
    {
        public decimal Total { get; set; }
        public decimal TotalTransactions { get; set; }

        public TagSummaryResultsViewModel(ICollection<TagSummaryViewModel> lines)
        {
            Lines = lines;
            TotalLines = lines.Count;
            Total = lines.Sum(l => l.Spend);
            TotalTransactions = lines.Sum(l => l.TransactionCount);
        }
    }
}
