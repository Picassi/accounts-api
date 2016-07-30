namespace Picassi.Core.Accounts.ViewModels.Categories
{
    public class CategorySummaryResultsViewModel : ResultsViewModel<CategorySummaryViewModel>
    {
        public decimal Total { get; set; }
        public decimal TotalTransactions { get; set; }
    }
}
