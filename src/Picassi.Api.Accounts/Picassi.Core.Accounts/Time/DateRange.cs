using System;

namespace Picassi.Core.Accounts.Time
{
    public class DateRange
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public DateRange() {  }

        public DateRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }
    }

    public class OpenEndedDateRange
    {
        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }
    }

}
