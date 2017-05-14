using System;
using Picassi.Core.Accounts.Enums;
using Picassi.Core.Accounts.Time.Periods;

namespace Picassi.Core.Accounts.Models.Categories
{
    public class CategoriesQueryModel
    {
        public string Name { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string PageSize { get; set; }
        public string PageNumber { get; set; }
        public int[] AccountIds { get; set; } 
        public SummaryReportType ReportType { get; set; }
        public PeriodType Frequency { get; set; }
        public string SortBy { get; set; }
        public bool SortAscending { get; set; }
        public bool AllAccounts { get; set; }
    }
}
