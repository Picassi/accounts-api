using System;

namespace Picassi.Core.Accounts.Models.Accounts
{
    public class AccountSummaryViewModel
    {
        public int AccountId { get; set; }

        public string AccountName { get; set; }

        public DateTime? LastUpdated { get; set; }

        public DateTime? SnapshotDate { get; set; }

        public DateTime? ComparisonDate { get; set; }

        public decimal Balance { get; set; }

        public decimal TotalDebit { get; set; }

        public decimal TotalCredit { get; set; }

        public decimal TotalChange { get; set; }

    }

    public class WealthSummaryViewModel
    {
        public DateTime? SnapshotDate { get; set; }

        public DateTime? ComparisonDate { get; set; }

        public decimal Balance { get; set; }

        public decimal TotalDebit { get; set; }

        public decimal TotalCredit { get; set; }

        public decimal TotalChange { get; set; }

    }

}
