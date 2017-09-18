using System.Collections.Generic;

namespace Picassi.Api.Accounts.Contract.Calendar
{
    public class CalendarViewModel
    {
        public string Title { get; set; }
        public IList<CalendarColumn> Columns { get; set; } = new List<CalendarColumn>();
        public IList<CalendarRow> Rows { get; set; } = new List<CalendarRow>();
    }
}
