using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;

namespace Picassi.Core.Accounts.Models.Categories
{
    public class CategorySummaryViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public decimal Spend { get; set; }
        public decimal TransactionCount { get; set; }

        public CategorySummaryViewModel(Category category, IList<Transaction> list)
        {
            Id = category?.Id;
            Name = category?.Name ?? "Uncategorised";
            Spend = list.Select(x => x.Amount).DefaultIfEmpty(0).Sum();
            TransactionCount = list.Count;
        }
    }
}
