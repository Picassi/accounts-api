using System.Collections.Generic;
using System.Linq;

namespace Picassi.Core.Accounts.Models.Categories
{
    public class CategorySummaryResultsViewModel : ResultsViewModel<CategorySummaryViewModel>
    {
        public decimal Total { get; set; }
        public decimal TotalTransactions { get; set; }

        public CategorySummaryResultsViewModel(ICollection<CategorySummaryViewModel> lines)
        {
            Lines = lines;
            TotalLines = lines.Count;
            Total = lines.Select(l => l.Spend).DefaultIfEmpty(0).Sum();
            TotalTransactions = lines.Select(l => l.TransactionCount).DefaultIfEmpty(0).Sum();
        }
    }
}
