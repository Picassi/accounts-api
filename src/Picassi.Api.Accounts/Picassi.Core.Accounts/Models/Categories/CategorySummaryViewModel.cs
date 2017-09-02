using System;

namespace Picassi.Core.Accounts.Models.Categories
{
    public class CategorySummaryViewModel
    {
        public int? CategoryId { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public decimal Budget { get; set; }

        public decimal TotalDebit { get; set; }

        public decimal TotalCredit { get; set; }

        public decimal TotalChange { get; set; }

    }
}
