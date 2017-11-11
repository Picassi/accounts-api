using System;

namespace Picassi.Core.Accounts.Models.ModelledTransactions
{
    public class ModelledTransactionQueryModel
    {
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
