using System.Linq;
using Picassi.Core.Accounts.Exceptions;

namespace Picassi.Core.Accounts.Services.Transactions
{
    public interface ICheckForDuplicates
    {
        void Validate(CreateTransactionRequest request, CreateTransactionContext context);
    }

    public class CheckForDuplicates : ICheckForDuplicates
    {
        public void Validate(CreateTransactionRequest request, CreateTransactionContext context)
        {
            if (context.PotentiallyConflictingTransactions.Any(transaction =>
                transaction.Description == request.Description &&
                transaction.Date == request.Date &&
                transaction.Amount == request.Amount))
            {
                throw new InvalidRequestException("Duplicate transaction exists");
            }
        }
    }
}
