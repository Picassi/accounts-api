using Picassi.Api.Accounts.Contract.Enums;

namespace Picassi.Core.Accounts.Models.AssignmentRules
{
    public class AssignmentRuleQueryModel
    {
        public int[] Accounts { get; set; }
        public int[] Categories { get; set; }
        public string Text { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public AssignmentRuleType[] Types { get; set; }
    }
}
