using System.Collections.Generic;

namespace Picassi.Api.Accounts.Contract.Calendar
{
    public class CalendarRow
    {
        public string Name { get; set; }
        public string Column { get; set; }
        public IList<CalendarCell> Cells { get; set; }
    }
}