using System.Collections.Generic;

namespace Picassi.Core.Accounts.Services.Transactions
{
    public interface IBalanceCalculator
    {
        void SetBalances(IList<TransactionUploadRecord> uploads, CreateTransactionContext context);
    }

    public class BalanceCalculator : IBalanceCalculator
    {
        public void SetBalances(IList<TransactionUploadRecord> uploads, CreateTransactionContext context)
        {
        }
    }
}
