using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services.Projections.BudgetProjectors;
using Picassi.Core.Accounts.Services.Projections.FlowProjectors;

namespace Picassi.Core.Accounts.Services.Projections
{
    public interface IAccountProjector
    {
        bool ProjectAccounts(ProjectionGenerationParameters projectionGenerationParameters);
    }

    public class AccountProjector : IAccountProjector
    {
        private readonly IAccountsDatabaseProvider _dataContext;
        private readonly IFlowProjectorProvider _flowProjectorProvider;
        private readonly IBudgetProjectorProvider _budgetProjectorProvider;
        private readonly IProjectionBalanceService _projectionBalanceService;

        public AccountProjector(IAccountsDatabaseProvider dataContext, IFlowProjectorProvider flowProjectorProvider, IBudgetProjectorProvider budgetProjectorProvider, IProjectionBalanceService projectionBalanceService)
        {
            _dataContext = dataContext;
            _flowProjectorProvider = flowProjectorProvider;
            _budgetProjectorProvider = budgetProjectorProvider;
            _projectionBalanceService = projectionBalanceService;
        }

        public bool ProjectAccounts(ProjectionGenerationParameters parms)
        {
            var start = parms?.Start ?? DateTime.Now;
            var end = parms?.End ?? DateTime.Now.AddYears(1);

            RemovePreviouslyCalculatedTransactions();
            AddUpdatedTransactions(start, end);
            UpdateBalances(start);
            _dataContext.GetDataContext().SaveChanges();

            return true;
        }

        private void RemovePreviouslyCalculatedTransactions()
        {
            foreach (var transaction in _dataContext.GetDataContext().ModelledTransactions)
            {
                _dataContext.GetDataContext().ModelledTransactions.Remove(transaction);
            }
        }

        private void AddUpdatedTransactions(DateTime start, DateTime end)
        {
            var transactions = GenerateTransactions(start, end);
            transactions = GenerateBudgets(start, end, transactions);
            AddOrdinalValues(transactions);

            foreach (var t in transactions)
            {
                _dataContext.GetDataContext().ModelledTransactions.Add(t);

            }
            _dataContext.GetDataContext().SaveChanges();
        }

        private IList<ModelledTransaction> GenerateTransactions(DateTime start, DateTime end)
        {
            return _dataContext.GetDataContext().ScheduledTransactions
                .ToList()
                .SelectMany(flow => GetTransactionsForFlow(flow, start, end))
                .ToList();
        }

        private IList<ModelledTransaction> GenerateBudgets(DateTime start, DateTime end, IList<ModelledTransaction> existingTransactions)
        {
            return _dataContext.GetDataContext().Budgets
                .ToList()
                .SelectMany(budget => GetTransactionsForBudget(budget, start, end, existingTransactions))
                .Union(existingTransactions)
                .OrderBy(transaction => transaction.Date)
                .ToList();
        }

        private IEnumerable<ModelledTransaction> GetTransactionsForFlow(ScheduledTransaction flow, DateTime start, DateTime end)
        {
            var projector = _flowProjectorProvider.GetProjectorForFlow(flow);
            return projector.GetProjectionsForFlow(flow, start, end);
        }

        private IEnumerable<ModelledTransaction> GetTransactionsForBudget(Budget budget, DateTime start, DateTime end, IList<ModelledTransaction> existingTransactions)
        {
            var projector = _budgetProjectorProvider.GetProjectorForFlow(budget);
            return projector.GetProjectionsForBudget(budget, start, end, existingTransactions);
        }

        private static void AddOrdinalValues(IEnumerable<ModelledTransaction> transactions)
        {
            var ordinal = 1;
            foreach (var transaction in transactions)
            {
                transaction.Ordinal = ordinal++;
            }
        }

        private void UpdateBalances(DateTime start)
        {
            _projectionBalanceService.SetTransactionBalances(start);
        }
    }
}
