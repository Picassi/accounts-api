using System;

namespace Picassi.Core.Accounts.Models.Categories
{
    public class CategorySpendingSummaryViewModel
    {
        public int? CategoryId { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public decimal WeeklyAverage { get; set; }

        public decimal MonthlyAverage { get; set; }

        public decimal Total { get; set; }
    }
}