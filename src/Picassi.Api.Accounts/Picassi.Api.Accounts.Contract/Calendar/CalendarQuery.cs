using System;

namespace Picassi.Api.Accounts.Contract.Calendar
{
    public class CalendarQuery
    {
        public DateTime? Start { get; set; }
        public ReportingPeriod PanelPeriod { get; set; }
        public ReportingPeriod RowPeriod { get; set; }
        public ReportingPeriod CellPeriod { get; set; }
    }
}