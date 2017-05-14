using System;
using System.Linq;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.Statements;

namespace Picassi.Core.Accounts.Services.Reports
{
    public interface IStatementService
    {
        StatementModel GetStatement(int accountId, StatementQueryModel query);
    }

    public class StatementService : IStatementService
    {
        private readonly IAccountBalanceService _accountBalanceService;
        private readonly ITransactionsDataService _transactionQueryService;
        private readonly IStatementViewModelFactory _statementViewModelFactory;

        public StatementService(IAccountBalanceService accountBalanceService, ITransactionsDataService transactionQueryService, IStatementViewModelFactory statementViewModelFactory)
        {
            _accountBalanceService = accountBalanceService;
            _transactionQueryService = transactionQueryService;
            _statementViewModelFactory = statementViewModelFactory;
        }

        public StatementModel GetStatement(int accountId, StatementQueryModel query)
        {
            var transactions = _transactionQueryService.Query(query.GetTransactionQuery(accountId)).ToList();
            var startDate = query.DateFrom ?? (transactions.Any() ? transactions.Min(x => x.Date) : (DateTime?) null);
            var startBalance = startDate == null ? 0 : _accountBalanceService.GetAccountBalance(accountId, (DateTime)startDate);
            return _statementViewModelFactory.CompileStatement(accountId, startBalance, transactions, query.PageNumber, query.PageSize);
        }
    }
}
