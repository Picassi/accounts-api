using System;
using System.Collections.Generic;

namespace Picassi.Core.Accounts.Models.Budgets
{
    public class BudgetsQueryModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public IList<int> Accounts { get; set; }
        public IList<int> Categories { get; set; }
        public string PageSize { get; set; }
        public string PageNumber { get; set; }
    }
}
