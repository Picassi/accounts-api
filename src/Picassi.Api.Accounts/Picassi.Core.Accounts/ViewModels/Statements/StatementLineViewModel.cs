using System;

namespace Picassi.Core.Accounts.ViewModels.Statements
{
    public class StatementLineViewModel
    {
        public int TransactionId { get; set; }

        public int? LinkedAccountId { get; set; }

        public DateTime? Date { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public decimal Credit { get; set; }

        public decimal Debit { get; set; }

        public decimal Balance { get; set; }
    }
}
