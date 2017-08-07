using System.Collections.Generic;
using Picassi.Core.Accounts.Services.Charts;

namespace Picassi.Core.Accounts.Models
{
    public class PeriodSummaryDataSeries
    {
        public string Name { get; set; }
        public IList<TransactionPeriodSummary> Data { get; set; }
    }
}