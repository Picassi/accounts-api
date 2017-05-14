using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Enums;
using Picassi.Core.Accounts.Time;

namespace Picassi.Core.Accounts.Services.Projections.FlowProjectors
{
    public class MonthlyFlowProjector : IFlowProjector
    {
        public FlowUserType Type => FlowUserType.Monthly;

        private readonly IMonthlyTransactionMapper _monthlyTransactionMapper;

        public MonthlyFlowProjector(IMonthlyTransactionMapper monthlyTransactionMapper)
        {
            _monthlyTransactionMapper = monthlyTransactionMapper;
        }

        public IEnumerable<ModelledTransaction> GetProjectionsForFlow(ScheduledTransaction flow, DateTime start, DateTime end)
        {            
            var monthlyTransaction = _monthlyTransactionMapper.Map(flow);

            if (monthlyTransaction.Start != null && monthlyTransaction.Start.Value > start)
                start = monthlyTransaction.Start.Value;
            if (monthlyTransaction.End != null && monthlyTransaction.End.Value < end)
                end = monthlyTransaction.End.Value;

            return start > end ? new List<ModelledTransaction>() : GetTransactions(start, end, monthlyTransaction);
        }

        private IEnumerable<ModelledTransaction> GetTransactions(DateTime start, DateTime end, MonthlyTransactionViewModel monthlyTransaction)
        {
            var transactions = DateUtilities.GetAllMatchingDaysOfTheMonth(start, end, monthlyTransaction.DayOfTheMonth)
                .Select(date => BuildTransaction(monthlyTransaction, date)).ToList();

            if (monthlyTransaction.TargetAccountId != null)
            {
                var additionTransactions = transactions.Select(
                        trans => GetBalancingTransaction(trans, monthlyTransaction.TargetAccountId.Value)).ToList();
                transactions.AddRange(additionTransactions);
            }

            return transactions;
        }

        private ModelledTransaction BuildTransaction(MonthlyTransactionViewModel monthlyTransaction, DateTime date)
        {
            return new ModelledTransaction
            {
                Amount = monthlyTransaction.Amount,
                Date = date,
                Description = monthlyTransaction.Description,
                ScheduledTransactionId = monthlyTransaction.Id,
                AccountId = monthlyTransaction.AccountId,
            };
        }

        private static ModelledTransaction GetBalancingTransaction(ModelledTransaction transaction, int accountId)
        {
            return new ModelledTransaction
            {
                Amount = -transaction.Amount,
                Date = transaction.Date,
                Description = transaction.Description,
                ScheduledTransactionId = transaction.ScheduledTransactionId,
                AccountId = accountId
            };
        }
    }
}
