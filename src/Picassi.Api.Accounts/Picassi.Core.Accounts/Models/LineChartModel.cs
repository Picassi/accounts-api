using System.Collections.Generic;
using Picassi.Core.Accounts.Services.Charts;

namespace Picassi.Core.Accounts.Models
{
    public class LineChartModel
    {
        public IList<PeriodSummaryDataSeries> Series { get; set; }
        public IEnumerable<TransactionPeriodSummary> Total { get; set; }
        public IList<string> Labels { get; set; }
        public string Type { get; set; }
    }
}
