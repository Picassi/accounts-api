namespace Picassi.Core.Accounts.ViewModels.Categories
{
    public class CategorySummaryResult
    {
        public int AccountId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Total { get; set; }
        public decimal Amount { get; set; }
    }
}