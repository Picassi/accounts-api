using System;

namespace Picassi.Core.Accounts.Services.Projections.FlowProjectors
{
    public class MonthlyTransactionViewModel
    {
        public int Id { get; set; }

        public int ProjectionId { get; set; }

        public string Description { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public int AccountId { get; set; }

        public int? TargetAccountId { get; set; }

        public int? CategoryId { get; set; }

        public decimal Amount { get; set; }

        public int DayOfTheMonth { get; set; }
    }
}
