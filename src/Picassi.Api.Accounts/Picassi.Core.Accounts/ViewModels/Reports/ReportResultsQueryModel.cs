using System;
using System.Collections.Generic;
using Picassi.Core.Accounts.Enums;
using Picassi.Core.Accounts.Time.Periods;

namespace Picassi.Core.Accounts.ViewModels.Reports
{
    public class ReportResultsQueryModel
    {
        public ICollection<int> Accounts { get; set; } 

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public PeriodType Period { get; set; }

        public CategorySummaryReportType Type { get; set; }
    }
}
