using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services.Reports;

namespace Picassi.Core.Accounts.Services.Projections
{
    public interface IProjectionBalanceService
    {
        void SetTransactionBalances(DateTime date);
    }

    public class ProjectionBalanceService : IProjectionBalanceService
    {
        private readonly IAccountsDataContext _projectionsDataContext;
        private readonly IAccountBalanceService _accountBalanceService;

        public ProjectionBalanceService(IAccountsDataContext projectionsDataContext, IAccountBalanceService accountBalanceService)
        {
            _projectionsDataContext = projectionsDataContext;
            _accountBalanceService = accountBalanceService;
        }

        public void SetTransactionBalances(DateTime date)
        {
            foreach (var account in _projectionsDataContext.Accounts.ToList())
            {
                SetTransactionBalances(account.Id, date);
            }
        }

        private void SetTransactionBalances(int accountId, DateTime date)
        {
            var startBalance = _accountBalanceService.GetAccountBalance(accountId, date);
            SetTransactionBalances(accountId, date, startBalance);
        }

        public void SetTransactionBalances(int accountId, DateTime date, decimal initialBalance)
        {
            SetBalanceForward(GetTransactionsAfterTransaction(accountId), initialBalance);

            _projectionsDataContext.SaveChanges();
        }

        private IList<ModelledTransaction> GetTransactionsAfterTransaction(int accountId)
        {
            return _projectionsDataContext.ModelledTransactions
                .Where(x => x.AccountId == accountId)
                .OrderBy(x => x.Date)
                .ThenBy(x => x.Ordinal)
                .ToList();
        }

        private static void SetBalanceForward(IEnumerable<ModelledTransaction> transactions, decimal initialBalance)
        {
            foreach (var transaction in transactions)
            {
                initialBalance = initialBalance + transaction.Amount;
                transaction.Balance = initialBalance;
            }
        }
    }
}
