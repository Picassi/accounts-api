using System;
using Picassi.Core.Accounts.Models.Transactions;

namespace Picassi.Core.Accounts.Models.Statements
{
    public class StatementQueryModel
    {
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public string Text { get; set; }
        public int[] Categories { get; set; }
        public bool ShowUncategorised { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public SimpleTransactionQueryModel GetTransactionQuery(int accountId)
        {
            return new SimpleTransactionQueryModel { AccountId = accountId, DateFrom = DateFrom, DateTo = DateTo };
        }
    }
}
