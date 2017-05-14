using System;
using Picassi.Core.Accounts.DAL.Entities;

namespace Picassi.Core.Accounts.Models.ScheduledTransactions
{
    public class ScheduledTransactionViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int? FromId { get; set; }

        public int? ToId { get; set; }

        public int? CategoryId { get; set; }

        public int? EventId { get; set; }

        public decimal Amount { get; set; }

        public DateTime? Date { get; set; }

        public int? DaysBefore { get; set; }

        public Period? Recurrence { get; set; }

        public int? RecurrenceDayOfMonth { get; set; }

        public int? RecurrenceWeekOfMonth { get; set; }

        public int? RecurrenceDayOfWeek { get; set; }
    }
}
