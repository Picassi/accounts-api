using System;
using Picassi.Core.Accounts.Time.Periods;

namespace Picassi.Core.Accounts.ViewModels.Categories
{
    public class CategoriesQueryModel
    {
        public string Name { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string PageSize { get; set; }
        public string PageNumber { get; set; }
        public PeriodType Frequency { get; set; }
        public int[] AccountIds { get; set; }
    }
}
