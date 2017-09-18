using Picassi.Core.Accounts.Time.Periods;

namespace Picassi.Core.Accounts.Models.Budgets
{
    public class BudgetModel
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public PeriodType Period { get; set; }

        public int AggregationPeriod { get; set; }

        public int CategoryId { get; set; }

        public int AccountId { get; set; }
    }
}
