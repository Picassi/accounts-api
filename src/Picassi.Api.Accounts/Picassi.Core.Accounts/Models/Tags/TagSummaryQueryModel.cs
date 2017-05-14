using System.Collections.Generic;
using Picassi.Core.Accounts.Enums;
using Picassi.Core.Accounts.Time;

namespace Picassi.Core.Accounts.Models.Tags
{
    public class TagSummaryQueryModel
    {
        public TagModel Tag { get; set; }        
        public DateRange DateRange { get; set; }
        public PeriodDefinition AverageSpendPeriod { get; set; }
        public List<int> AccountIds { get; set; }
        public SummaryReportType ReportType { get; set; }
    }
}
