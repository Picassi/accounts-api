namespace Picassi.Core.Accounts.Models.Tags
{
    public class TagSummaryResult
    {
        public int AccountId { get; set; }
        public int TagId { get; set; }
        public string TagName { get; set; }
        public int Total { get; set; }
        public decimal Amount { get; set; }
    }
}