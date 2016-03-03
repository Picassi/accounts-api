using System;

namespace Picassi.Core.Accounts.ViewModels.Accounts
{
    public class AccountSummaryViewModel
    {
        public int AccountId { get; set; }

        public DateTime? LastUpdated { get; set; }

        public DateTime? SnapshotDate { get; set; }

        public DateTime? ComparisonDate { get; set; }

        public decimal Balance { get; set; }

        public decimal TotalDebit { get; set; }

        public decimal TotalCredit { get; set; }

        public decimal TotalChange { get; set; }

    }
}
