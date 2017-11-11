using System;
using System.Linq;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Services.Reports;

namespace Picassi.Core.Accounts.Services.Transactions
{
    public interface ICreateTransactionContextBuilder
    {
        CreateTransactionContext BuildContext(int accountId, DateTime firstUploadDate);
    }

    public class CreateTransactionContextBuilder : ICreateTransactionContextBuilder
    {

        private readonly IAccountBalanceService _accountBalanceService;
        private readonly ITransactionsDataService _transactionsDataService;

        public CreateTransactionContextBuilder(IAccountBalanceService accountBalanceService, ITransactionsDataService transactionsDataService)
        {
            _accountBalanceService = accountBalanceService;
            _transactionsDataService = transactionsDataService;
        }

        public CreateTransactionContext BuildContext(int accountId, DateTime firstUploadDate)
        {
            var balance = _accountBalanceService.GetAccountBalance(accountId, firstUploadDate);
            var conflicts = _transactionsDataService.Query(accounts: new[] { accountId }, dateFrom: firstUploadDate);

            return new CreateTransactionContext
            {
                BalanceBeforeCreation = balance,
                PotentiallyConflictingTransactions = conflicts.ToList()
            };
        }
    }
}
