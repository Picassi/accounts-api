using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;

namespace Picassi.Core.Accounts.Models.Categories
{
    public class CategoryAccountSummaryResult
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Total { get; set; }
        public decimal Amount { get; set; }

        public CategoryAccountSummaryResult(Category category, ICollection<CategorySummaryResult> list)
        {
            CategoryId = category.Id;
            CategoryName = category.Name;
            Total = list.Sum(c => c.Total);
            Amount = list.Sum(c => c.Amount);
        }
    }
}