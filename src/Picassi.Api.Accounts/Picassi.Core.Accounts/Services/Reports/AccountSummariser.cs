using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Api.Accounts.Contract.Transactions;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.Accounts;
using Picassi.Core.Accounts.Models.Transactions;
using Picassi.Core.Accounts.Services.Accounts;

namespace Picassi.Core.Accounts.Services.Reports
{
    public interface IAccountSummariser
    {
        WealthSummaryViewModel GetWealthSummary(DateTime from, DateTime to);
        AccountSummaryViewModel GetAccountSummary(int accountId, DateTime from, DateTime to);
        IEnumerable<AccountSummaryViewModel> GetAccountSummaries(DateTime from, DateTime to);
    }

    public class AccountSummariser : IAccountSummariser
    {
        private readonly IAccountDataService _accountDataService;
        private readonly ITransactionsDataService _transactionsDataService;
        private readonly IAccountBalanceService _accountBalanceService;
        private readonly IAccountStatusChecker _statusChecker;

        public AccountSummariser(IAccountDataService accountDataService, ITransactionsDataService transactionsDataService, IAccountBalanceService accountBalanceService, IAccountStatusChecker statusChecker)
        {
            _accountDataService = accountDataService;
            _transactionsDataService = transactionsDataService;
            _accountBalanceService = accountBalanceService;
            _statusChecker = statusChecker;
        }

        public WealthSummaryViewModel GetWealthSummary(DateTime from, DateTime to)
        {
            var summaries = GetAccountSummaries(from, to).ToList();
            var credit = summaries.Sum(x => x.TotalCredit);
            var debit = summaries.Sum(x => x.TotalDebit);
            var change = credit - debit;
            var balance = summaries.Sum(x => x.Balance);
            return new WealthSummaryViewModel
            {
                TotalCredit = credit,
                TotalDebit = debit, 
                TotalChange = change,
                Balance = balance
            };
        }

        public AccountSummaryViewModel GetAccountSummary(int accountId, DateTime from, DateTime to)
        {
            var accountIds = new[] { accountId};
            var account = _accountDataService.Get(accountId);
            var transactions = _transactionsDataService.Query(accounts: accountIds, dateFrom: from, dateTo: to).ToList();
            return CompileAccountSummary(accountId, account.Name, from, to, transactions);
        }

        public IEnumerable<AccountSummaryViewModel> GetAccountSummaries(DateTime from, DateTime to)
        {
            var accounts = _accountDataService.Query(new AccountQueryModel());
            var transactions = _transactionsDataService.Query(dateFrom: from, dateTo: to).ToList();
            return accounts.Select(x => CompileAccountSummary(x.Id, x.Name, from, to, transactions.Where(t => t.AccountId == x.Id).ToList()));
        }

        private AccountSummaryViewModel CompileAccountSummary(int accountId, string accountName, DateTime from, DateTime to, IList<TransactionModel> transactions)
        {
            var credit = transactions.Where(x => x.Amount > 0).Select(x => x.Amount).DefaultIfEmpty(0).Sum();
            var debit = transactions.Where(x => x.Amount < 0).Select(x => x.Amount).DefaultIfEmpty(0).Sum();

            var balance = transactions.Any()
                ? transactions.OrderBy(x => x.Date).ThenBy(x => x.Ordinal).Select(x => x.Balance).Last()
                : _accountBalanceService.GetAccountBalance(accountId, to);

            var lastUpdated = _statusChecker.LastUpdated(accountId);

            return new AccountSummaryViewModel
            {
                AccountId = accountId,
                AccountName = accountName,
                ComparisonDate = @from,
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
