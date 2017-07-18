using System;
using System.Linq;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.Accounts;
using Picassi.Core.Accounts.Services.Accounts;

namespace Picassi.Core.Accounts.Services.Reports
{
    public interface IAccountSummariser
    {
        AccountSummaryViewModel GetAccountSummary(int? accountId, DateTime? from, DateTime? to);
    }

    public class AccountSummariser : IAccountSummariser
    {
        private readonly ITransactionsDataService _transactionsDataService;
        private readonly IAccountBalanceService _accountBalanceService;
        private readonly IAccountStatusChecker _statusChecker;

        public AccountSummariser(ITransactionsDataService transactionsDataService, IAccountBalanceService accountBalanceService, IAccountStatusChecker statusChecker)
        {
            _transactionsDataService = transactionsDataService;
            _accountBalanceService = accountBalanceService;
            _statusChecker = statusChecker;
        }

        public AccountSummaryViewModel GetAccountSummary(int? accountId, DateTime? from, DateTime? to)
        {
            var accounts = accountId == null ? null : new[] {(int) accountId};
            var transactions = _transactionsDataService.Query(accounts: accounts, dateFrom: from, dateTo: to).ToList();
            var credit = transactions.Where(x => x.Amount > 0).Select(x => x.Amount).DefaultIfEmpty(0).Sum();
            var debit = transactions.Where(x => x.Amount < 0).Select(x => x.Amount).DefaultIfEmpty(0).Sum();

            var balance = transactions.Any() ?
                transactions.OrderBy(x => x.Date).ThenBy(x => x.Ordinal).Select(x => x.Balance).Last() : 
                _accountBalanceService.GetAccountBalance(accountId, to ?? DateTime.Today);

            var lastUpdated = _statusChecker.LastUpdated(accountId);

            return new AccountSummaryViewModel
            {
                AccountId = accountId,
                ComparisonDate = from,
                SnapshotDate = to,
                Balance = balance,
                TotalDebit = debit,
                TotalCredit = credit,
                TotalChange = credit - debit,
                LastUpdated = lastUpdated
            };

        }
    }
}
