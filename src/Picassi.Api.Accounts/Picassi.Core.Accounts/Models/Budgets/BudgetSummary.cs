namespace Picassi.Core.Accounts.Models.Budgets
{
    public class BudgetSummary
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Spent { get; set; }
        public decimal? Budget { get; set; }
    }
}