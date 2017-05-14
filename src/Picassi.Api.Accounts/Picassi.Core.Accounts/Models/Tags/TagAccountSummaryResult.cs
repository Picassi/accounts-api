using System.Collections.Generic;
using System.Linq;

namespace Picassi.Core.Accounts.Models.Tags
{
    public class TagAccountSummaryResult
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
        public int Total { get; set; }
        public decimal Amount { get; set; }

        public TagAccountSummaryResult(int tagId, string tagName, ICollection<TagSummaryResult> list)
        {
            TagId = tagId;
            TagName = tagName;
            Total = list.Sum(c => c.Total);
            Amount = list.Sum(c => c.Amount);
        }
    }
}