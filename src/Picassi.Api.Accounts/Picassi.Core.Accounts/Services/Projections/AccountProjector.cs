using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL;
using Picassi.Core.Accounts.DAL.Entities;
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
        private readonly IProjectionBalanceService _projectionBalanceService;

        public AccountProjector(IAccountsDatabaseProvider dataContext, IFlowProjectorProvider flowProjectorProvider, IProjectionBalanceService projectionBalanceService)
        {
            _dataContext = dataContext;
            _flowProjectorProvider = flowProjectorProvider;
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
            foreach (var t in transactions)
            {
                _dataContext.GetDataContext().ModelledTransactions.Add(t);

            }
            _dataContext.GetDataContext().SaveChanges();
        }

        private IEnumerable<ModelledTransaction> GenerateTransactions(DateTime start, DateTime end)
        {
            var transactions = _dataContext.GetDataContext().ScheduledTransactions
                .ToList()
                .SelectMany(flow => GetTransactionsForFlow(flow, start, end))
                .OrderBy(flow => flow.Date);

            AddOrdinalValues(transactions);

            return transactions;
        }

        private static void AddOrdinalValues(IEnumerable<ModelledTransaction> transactions)
        {
            var ordinal = 1;
            foreach (var transaction in transactions)
            {
                transaction.Ordinal = ordinal++;
            }
        }

        private IEnumerable<ModelledTransaction> GetTransactionsForFlow(ScheduledTransaction flow, DateTime start, DateTime end)
        {
            var projector = _flowProjectorProvider.GetProjectorForFlow(flow);
            return projector.GetProjectionsForFlow(flow, start, end);
        }

        private void UpdateBalances(DateTime start)
        {
            _projectionBalanceService.SetTransactionBalances(start);
        }
    }
}
