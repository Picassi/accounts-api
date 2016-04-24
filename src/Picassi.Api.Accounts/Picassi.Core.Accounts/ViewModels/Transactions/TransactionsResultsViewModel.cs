using System.Collections.Generic;

namespace Picassi.Core.Accounts.ViewModels.Transactions
{
    public class TransactionsResultsViewModel
    {
        public IEnumerable<TransactionViewModel> Lines { get; set; }
        public int TotalLines { get; set; }
    }
}
