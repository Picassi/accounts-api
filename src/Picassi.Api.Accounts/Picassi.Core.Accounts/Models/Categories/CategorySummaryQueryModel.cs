using System.Collections.Generic;
using Picassi.Api.Accounts.Contract.Enums;
using Picassi.Core.Accounts.Time;

namespace Picassi.Core.Accounts.Models.Categories
{
    public class CategorySummaryQueryModel
    {
        public CategoryModel Category { get; set; }        
        public DateRange DateRange { get; set; }
        public PeriodDefinition AverageSpendPeriod { get; set; }
        public List<int> AccountIds { get; set; }
        public SummaryReportType ReportType { get; set; }
    }
}
