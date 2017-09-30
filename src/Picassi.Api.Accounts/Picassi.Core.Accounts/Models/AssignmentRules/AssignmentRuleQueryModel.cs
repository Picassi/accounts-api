namespace Picassi.Core.Accounts.Models.AssignmentRules
{
    public class AssignmentRuleQueryModel
    {
        public int[] AccountIds { get; set; }
        public int[] CategoryIds { get; set; }
        public string Name { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
