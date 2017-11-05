using System;

namespace Picassi.Api.Accounts.Contract.Transactions
{
    public class TransactionsQueryModel
    {
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public string Text { get; set; }
        public int[] Accounts { get; set; }
        public int[] Categories { get; set; }
        public bool? ShowAllCategorised { get; set; }
        public bool? ShowUncategorised { get; set; }
        public bool? ShowSubcategories { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string SortBy { get; set; }
        public bool? SortAscending { get; set; }
    }
}
