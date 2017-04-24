using System.Collections.Generic;
using System.Linq;

namespace Picassi.Core.Accounts.ViewModels.Categories
{
    public class CategoryAccountSummaryResult
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Total { get; set; }
        public decimal Amount { get; set; }

        public CategoryAccountSummaryResult(int categoryId, string categoryName, ICollection<CategorySummaryResult> list)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            Total = list.Sum(c => c.Total);
            Amount = list.Sum(c => c.Amount);
        }
    }
}