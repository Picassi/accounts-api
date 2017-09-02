using System;
using Picassi.Core.Accounts.Time.Periods;

namespace Picassi.Core.Accounts.Models.ScheduledTransactions
{
    public class ScheduledTransactionModel
    {
        public int Id { get; set; }

        public string Description { get; set; }    
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public int? ToId { get; set; }
        public string ToName { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? EventId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? Date { get; set; }
        public int? DaysBefore { get; set; }
        public PeriodType? Recurrence { get; set; }
        public int? RecurrenceDayOfMonth { get; set; }
        public int? RecurrenceWeekOfMonth { get; set; }
        public int? RecurrenceDayOfWeek { get; set; }
    }
}
