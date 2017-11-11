using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Picassi.Api.Accounts.Contract.Transactions;
using Picassi.Core.Accounts.DAL.Services;

namespace Picassi.Core.Accounts.Services.Transactions
{
    public interface IAccounStatementIntegrityService
    {
        Task OnTransactionsCreated(int accountId, IList<TransactionModel> createdTransactions);
        Task OnTransactionsCreated(int accountId, TransactionModel transaction);
    }

    public class AccountStatementIntegrityService : IAccounStatementIntegrityService
    {
        private readonly ITransactionSorter _transactionSorter;
        private readonly ITransactionBalanceAdjuster _balanceAdjuster;
        private readonly ITransactionsDataService _transactionsDataService;

        public AccountStatementIntegrityService(ITransactionSorter transactionSorter, ITransactionBalanceAdjuster balanceAdjuster, ITransactionsDataService transactionsDataService)
        {
            _transactionSorter = transactionSorter;
            _balanceAdjuster = balanceAdjuster;
            _transactionsDataService = transactionsDataService;
        }

        public async Task OnTransactionsCreated(int accountId, IList<TransactionModel> createdTransactions)
        {
            await _transactionSorter.UpdateStatementOrder(accountId);
            var baseTransaction = createdTransactions.OrderBy(x => x.Date).ThenBy(x => x.Ordinal).LastOrDefault();
            if (baseTransaction != null)
            {
                await UpdateStatement(baseTransaction.Id);
            }
        }

        public async Task OnTransactionsCreated(int accountId, TransactionModel transaction)
        {
            await _transactionSorter.UpdateStatementOrder(accountId);
            await UpdateStatement(transaction.Id);
        }

        private async Task UpdateStatement(int transactionId)
        {
            var model = _transactionsDataService.Get(transactionId);
            await _balanceAdjuster.UpdateStatementBalances(model);
        }
    }
}
