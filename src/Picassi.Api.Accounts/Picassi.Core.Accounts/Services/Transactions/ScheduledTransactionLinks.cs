using System.Collections.Generic;

namespace Picassi.Core.Accounts.Services.Transactions
{
    public interface IScheduledTransactionLinks
    {
        void LinkScheduledTransactions(IList<TransactionUploadRecord> uploads, CreateTransactionContext context);
    }

    public class ScheduledTransactionLinkLinks : IScheduledTransactionLinks
    {
        public void LinkScheduledTransactions(IList<TransactionUploadRecord> uploads, CreateTransactionContext context)
        {
        }
    }
}
