using Picassi.Core.Accounts.DAL.Entities;

namespace Picassi.Core.Accounts.Models.Budgets
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
