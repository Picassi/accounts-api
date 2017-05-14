using System.Linq;
using Picassi.Core.Accounts.Models.Accounts;
using Picassi.Core.Accounts.Models.Statements;
using Picassi.Core.Accounts.Services.Accounts;

namespace Picassi.Core.Accounts.Services.Reports
{
    public interface IAccountSummariser
    {
        AccountSummaryViewModel GetAccountSummary(AccountPeriodViewModel view);
        AccountsStatusViewModel GetAccountsSummary();
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
            var statement = _statementService.GetStatement(view.AccountId, new StatementQueryModel { DateFrom = view.From, DateTo = view.To });
            var credit = statement.Lines.Sum(x => x.Credit);
            var debit = statement.Lines.Sum(x => x.Debit);
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

        public AccountsStatusViewModel GetAccountsSummary()
        {
            return new AccountsStatusViewModel
            {
                LastUpdated = _statusChecker.LastUpdated(),
                MostOutOfDate = _statusChecker.MostOutOfDate()
            };
        }
    }
}
