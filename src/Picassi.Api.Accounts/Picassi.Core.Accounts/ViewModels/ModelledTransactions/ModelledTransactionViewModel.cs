using System;

namespace Picassi.Core.Accounts.ViewModels.ModelledTransactions
{
    public class ModelledTransactionViewModel
    {
        public int Id { get; set; }

        public int Ordinal { get; set; }

        public string Description { get; set; }

        public int? FromId { get; set; }

        public int? ToId { get; set; }

        public int? CategoryId { get; set; }

        public int? EventId { get; set; }

        public int? ScheduledTransactionId { get; set; }

        public decimal Amount { get; set; }

        public decimal Balance { get; set; }

        public DateTime Date { get; set; }
    }
}
