using System.Collections.Generic;

namespace Picassi.Core.Accounts.ViewModels.Categories
{
    public class CategorySummaryViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public Dictionary<int, decimal?> SpendingReport { get; set; }
        public decimal AverageSpend { get; set; }
    }
}
