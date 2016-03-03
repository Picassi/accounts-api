using System.Linq;
using Picassi.Core.Accounts.Services.Accounts;
using Picassi.Core.Accounts.ViewModels.Accounts;

namespace Picassi.Core.Accounts.Reports
{
    public interface IAccountSummariser
    {
        AccountSummaryViewModel GetAccountSummary(AccountPeriodViewModel view);
    }

    public class AccountSummaryViewModelFactory : IAccountSummariser
    {
        private readonly IStatementService _statementService;
        private readonly IAccountStatusChecker _statusChecker;

        public AccountSummaryViewModelFactory(IStatementService statementService, IAccountStatusChecker statusChecker)
        {
            _statementService = statementService;
            _statusChecker = statusChecker;
        }

        public AccountSummaryViewModel GetAccountSummary(AccountPeriodViewModel view)
        {
            var statement = _statementService.GetStatement(view, "transaction");
            var credit = statement.Lines.Sum(x => x.AmountIn);
            var debit = statement.Lines.Sum(x => x.AmountOut);
            var lastTransactions = statement.Lines.OrderBy(x => x.Date).LastOrDefault();

            return new AccountSummaryViewModel
            {
                AccountId = view.AccountId,
                ComparisonDate = view.From,
                SnapshotDate = view.To,
                Balance = lastTransactions?.Balance ?? statement.StartBalance,
                TotalDebit = debit,
                TotalCredit = credit,
                TotalChange = credit - debit,
                LastUpdated = _statusChecker.LastUpdated(view.AccountId)
            };

        }
    }
}
