using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Api.Accounts.Contract.Enums;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Time;
using Picassi.Core.Accounts.Time.Periods;

namespace Picassi.Core.Accounts.Services.Projections.FlowProjectors
{
    public class WeeklyFlowProjector : IFlowProjector
    {
        public PeriodType Type => PeriodType.Week;

        private readonly IWeeklyTransactionMapper _weeklyTransactionMapper;

        public WeeklyFlowProjector(IWeeklyTransactionMapper weeklyTransactionMapper)
        {
            _weeklyTransactionMapper = weeklyTransactionMapper;
        }

        public IEnumerable<ModelledTransaction> GetProjectionsForFlow(ScheduledTransaction flow, DateTime start, DateTime end)
        {            
            var weeklyTransaction = _weeklyTransactionMapper.Map(flow);

            if (weeklyTransaction.Start != null && weeklyTransaction.Start.Value > start)
                start = weeklyTransaction.Start.Value;
            if (weeklyTransaction.End != null && weeklyTransaction.End.Value < end)
                end = weeklyTransaction.End.Value;
            return start > end ? new List<ModelledTransaction>() : GetTransactions(start, end, weeklyTransaction);
        }

        private IEnumerable<ModelledTransaction> GetTransactions(DateTime start, DateTime end, WeeklyTransactionViewModel weeklyTransaction)
        {
            var transactions = DateUtilities.GetAllMatchingDaysOfTheWeek(start, end, weeklyTransaction.DayOfTheWeek)
                .Select(date => BuildTransaction(weeklyTransaction, date)).ToList();

            if (weeklyTransaction.TargetAccountId != null)
            {
                var additionTransactions = transactions.Select(
                        trans => GetBalancingTransaction(trans, weeklyTransaction.TargetAccountId.Value)).ToList();
                transactions.AddRange(additionTransactions);
            }

            return transactions;
        }

        private ModelledTransaction BuildTransaction(WeeklyTransactionViewModel weeklyTransaction, DateTime date)
        {
            return new ModelledTransaction
            {
                Amount = weeklyTransaction.Amount,
                Date = date,
                Description = weeklyTransaction.Description,
                ScheduledTransactionId = weeklyTransaction.Id,
                AccountId = weeklyTransaction.AccountId,
                CategoryId = weeklyTransaction.CategoryId
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
                AccountId = accountId,
                CategoryId = transaction.CategoryId
            };
        }

    }
}
