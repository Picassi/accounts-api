using System.Collections.Generic;
using Picassi.Core.Accounts.Time;

namespace Picassi.Core.Accounts.ViewModels.Categories
{
    public class CategorySummaryQueryModel
    {
        public CategoryViewModel Category { get; set; }        
        public DateRange DateRange { get; set; }
        public PeriodDefinition AverageSpendPeriod { get; set; }
        public List<int> AccountIds { get; set; }
    }
}
