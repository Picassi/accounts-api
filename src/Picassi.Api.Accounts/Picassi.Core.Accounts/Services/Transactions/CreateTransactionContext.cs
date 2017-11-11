using System.Collections.Generic;
using Picassi.Api.Accounts.Contract.Transactions;
using Picassi.Core.Accounts.Models.ScheduledTransactions;

namespace Picassi.Core.Accounts.Services.Transactions
{
    public class CreateTransactionContext
    {
        public decimal BalanceBeforeCreation { get; set; }

        public int AccountId { get; set; }
        public IList<TransactionModel> PotentiallyConflictingTransactions { get; set; }
        public IList<ScheduledTransactionModel> ScheduledTransactions { get; set; }
    }
}
