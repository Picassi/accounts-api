using System.Collections.Generic;
using System.Linq;

namespace Picassi.Core.Accounts.Models.Tags
{
    public class TagSummaryViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public decimal Spend { get; set; }
        public decimal TransactionCount { get; set; }

        public TagSummaryViewModel(int id, string name, ICollection<TagAccountSummaryResult> list)
        {
            Id = id;
            Name = name;
            Spend = list.Sum(x => x.Amount);
            TransactionCount = list.Sum(x => x.Total);
        }
    }
}
