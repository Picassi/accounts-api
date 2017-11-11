using Picassi.Api.Accounts.Contract.Transactions;
using Picassi.Core.Accounts.DAL.Services;

namespace Picassi.Core.Accounts.Services.Transactions
{
    public interface ICreateTransactionOperation
    {
        TransactionModel CreateTransaction(CreateTransactionRequest request, CreateTransactionContext context);
    }

    public class CreateTransactionOperation : ICreateTransactionOperation
    {
        private readonly ICheckForDuplicates _checkForDuplicates;
        private readonly ITransactionsDataService _transactionsDataService;

        public CreateTransactionOperation(ICheckForDuplicates checkForDuplicates, ITransactionsDataService transactionsDataService)
        {
            _checkForDuplicates = checkForDuplicates;
            _transactionsDataService = transactionsDataService;
        }


        public TransactionModel CreateTransaction(CreateTransactionRequest request, CreateTransactionContext context)
        {
            _checkForDuplicates.Validate(request, context);
            var model = BuildModel(request);
            return _transactionsDataService.Create(model);
        }

        private TransactionModel BuildModel(CreateTransactionRequest request)
        {
            return new TransactionModel
            {
                AccountId = request.AccountId,
                ToId = request.ToId,
                Amount = request.Amount,
                Date = request.Date,
                Description = request.Description,
                CategoryId = request.CategoryId,
                Ordinal = request.Ordinal,
                Balance = request.Balance ?? 0
            };
        }
    }
}
