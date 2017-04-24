using System.Collections.Generic;
using System.Linq;

namespace Picassi.Core.Accounts.ViewModels.Categories
{
    public class CategorySummaryResultsViewModel : ResultsViewModel<CategorySummaryViewModel>
    {
        public decimal Total { get; set; }
        public decimal TotalTransactions { get; set; }

        public CategorySummaryResultsViewModel(ICollection<CategorySummaryViewModel> lines)
        {
            Lines = lines;
            TotalLines = lines.Count;
            Total = lines.Sum(l => l.Spend);
            TotalTransactions = lines.Sum(l => l.TransactionCount);
        }
    }
}
