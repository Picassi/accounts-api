using System;
using Picassi.Core.Accounts.DbAccess.Transactions;
using Picassi.Core.Accounts.ViewModels.Accounts;
using Picassi.Core.Accounts.ViewModels.Statements;

namespace Picassi.Core.Accounts.Reports
{
    public interface IStatementService
    {
        StatementViewModel GetStatement(AccountPeriodViewModel view, string groupBy);
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

        public StatementViewModel GetStatement(AccountPeriodViewModel view, string groupBy)
        {
            var startBalance = (view.From != null)
                ? _accountBalanceService.GetAccountBalance(view.AccountId, (DateTime)view.From)
                : 0;
            var transactions = _transactionQueryService.Query(view);
            return _statementCompilerService.CompileStatement(view.AccountId, startBalance, transactions, groupBy);
        }
    }
}
