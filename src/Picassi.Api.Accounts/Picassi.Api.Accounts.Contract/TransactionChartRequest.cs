using System;
using Picassi.Api.Accounts.Contract.Enums;

namespace Picassi.Api.Accounts.Contract
{
    public class TransactionChartRequest
    {
        public int[] Accounts { get; set; }
        public int[] Categories { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public PeriodType? GroupBy { get; set; }
        public bool? IncludeSubcategories { get; set; }
    }
}
