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

        public CategorySummaryViewModel(Category category, ICollection<Transaction> list)
        {
            Id = category.Id;
            Name = category.Name;
            Spend = list.Sum(x => x.Amount);
            TransactionCount = list.Count;
        }
    }
}
