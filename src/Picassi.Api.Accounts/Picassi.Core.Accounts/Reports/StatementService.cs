using System;
using Picassi.Core.Accounts.DbAccess.Transactions;
using Picassi.Core.Accounts.ViewModels.Statements;
using Picassi.Core.Accounts.ViewModels.Transactions;

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
        private readonly IStatementCompilerService _statementCompilerService;

        public StatementService(IAccountBalanceService accountBalanceService, ITransactionQueryService transactionQueryService, IStatementCompilerService statementCompilerService)
        {
            _accountBalanceService = accountBalanceService;
            _transactionQueryService = transactionQueryService;
            _statementCompilerService = statementCompilerService;
        }

        public StatementViewModel GetStatement(int accountId, StatementQueryModel query)
        {
            var startBalance = (query.DateFrom != null)
                ? _accountBalanceService.GetAccountBalance(accountId, (DateTime)query.DateFrom)
                : 0;
            var transactions = _transactionQueryService.Query(query.GetTransactionQuery(accountId));                
            return _statementCompilerService.CompileStatement(accountId, startBalance, transactions);
        }
    }
}
