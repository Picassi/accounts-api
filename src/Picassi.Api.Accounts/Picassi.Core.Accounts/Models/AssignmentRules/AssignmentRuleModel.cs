using System;

namespace Picassi.Core.Accounts.Models.AssignmentRules
{
    public class AssignmentRuleModel
    {
        public int Id { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string DescriptionRegex { get; set; }

        public bool Enabled { get; set; }

        public int? Priority { get; set; }

        public int? CategoryId { get; set; }
    }
}
