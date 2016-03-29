using System;

namespace Picassi.Core.Accounts.ViewModels.Transactions
{
    public class TransactionsQueryModel
    {
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public string Text { get; set; }
        public int[] Accounts { get; set; }
        public int[] Categories { get; set; }
        public bool ShowUncategorised { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool? IncludeProvisional { get; set; }
        public bool? IncludeConfirmed { get; set; }        
    }
}
