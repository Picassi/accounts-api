namespace Picassi.Core.Accounts.ViewModels.Categories
{
    public class CategorySummaryViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public decimal? Spend { get; set; }
        public decimal TransactionCount { get; set; }
    }
}
