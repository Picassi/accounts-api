using System.Collections.Generic;

namespace Picassi.Core.Accounts.Models
{
    public class LineChartModel
    {
        public IList<PeriodSummaryDataSeries> Series { get; set; }
        public IList<string> Labels { get; set; }
        public string Type { get; set; }
    }
}
