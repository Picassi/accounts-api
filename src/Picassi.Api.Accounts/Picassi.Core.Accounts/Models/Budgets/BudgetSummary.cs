using Picassi.Api.Accounts.Contract.Enums;
using Picassi.Core.Accounts.Time.Periods;

namespace Picassi.Core.Accounts.Models.Budgets
{
    public class BudgetSummary
    {
        public int? BudgetId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Spent { get; set; }
        public decimal? Budget { get; set; }
        public decimal? BudgetAmount { get; set; }
        public PeriodType? BudgetPeriod { get; set; }
        public int? BudgetAggregationPeriod { get; set; }
        public decimal WeeklyAverage { get; set; }
        public decimal MonthlyAverage { get; set; }
    }
}