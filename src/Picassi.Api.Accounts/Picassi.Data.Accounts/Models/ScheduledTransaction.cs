using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Picassi.Data.Accounts.Models
{
    public class ScheduledTransaction
    {
        public int Id { get; set; }

        public string Description { get; set; }

        [ForeignKey("From")]
        public int? FromId { get; set; }

        [ForeignKey("To")]
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

        public virtual Account From { get; set; }

        public virtual Account To { get; set; }

        public virtual Category Category { get; set; }
    }
}
