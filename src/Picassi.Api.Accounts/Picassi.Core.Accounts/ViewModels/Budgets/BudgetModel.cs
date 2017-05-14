using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.ViewModels.Budgets
{
    public class BudgetModel
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public Period Period { get; set; }

        public int AggregationPeriod { get; set; }

        public int CategoryId { get; set; }
    }
}
