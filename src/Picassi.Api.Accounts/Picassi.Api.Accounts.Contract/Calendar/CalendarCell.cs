using System;
using System.Collections.Generic;

namespace Picassi.Api.Accounts.Contract.Calendar
{
    public class CalendarCell
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public IList<CalendarTransaction> Transactions { get; set; } 
    }

    public class CalendarTransaction
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}