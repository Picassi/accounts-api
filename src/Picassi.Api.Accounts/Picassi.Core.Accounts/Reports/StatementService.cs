using System;
using Picassi.Core.Accounts.DbAccess.Transactions;
using Picassi.Core.Accounts.ViewModels.Statements;

namespace Picassi.Core.Accounts.Reports
{
    public interface IStatementService
    {
        StatementViewModel GetStatement(int accountId, StatementQueryModel query);
    }

    public class StatementService : IStatementService
    {
        private readonly IAccountBalanceService _accountBalanceService;
        private readonly ITransactionQueryService _transactionQueryService;
        private readonly IStatementViewModelFactory _statementViewModelFactory;

        public StatementService(IAccountBalanceService accountBalanceService, ITransactionQueryService transactionQueryService, IStatementViewModelFactory statementViewModelFactory)
        {
            _accountBalanceService = accountBalanceService;
            _transactionQueryService = transactionQueryService;
            _statementViewModelFactory = statementViewModelFactory;
        }

        public StatementViewModel GetStatement(int accountId, StatementQueryModel query)
        {
            var startBalance = (query.DateFrom != null)
                ? _accountBalanceService.GetAccountBalance(accountId, (DateTime)query.DateFrom)
                : 0;
            var transactions = _transactionQueryService.Query(query.GetTransactionQuery(accountId));                
            return _statementViewModelFactory.CompileStatement(accountId, startBalance, transactions);
        }
    }
}
