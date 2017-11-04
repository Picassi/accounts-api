using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Api.Accounts.Contract.Enums;
using Picassi.Api.Accounts.Contract.Transactions;
using Picassi.Core.Accounts.Models.Transactions;
using Picassi.Core.Accounts.Time.Periods;

namespace Picassi.Core.Accounts.Services.Charts
{
    public interface ITransactionPeriodSummariser
    {
        IEnumerable<TransactionPeriodSummary> GetDataPoints(DateTime from, DateTime to, PeriodType period, IEnumerable<TransactionModel> transactions);
    }

    public class TransactionPeriodSummariser : ITransactionPeriodSummariser
    {
        public IEnumerable<TransactionPeriodSummary> GetDataPoints(DateTime from, DateTime to, PeriodType period, IEnumerable<TransactionModel> transactions)
        {
            switch (period)
            {
                case PeriodType.Second:
                case PeriodType.Minute:
                case PeriodType.Hour:
                case PeriodType.Day:
                    return GetDailyDataPoints(from, to, transactions);
                case PeriodType.Week:
                    return GetWeeklyDataPoints(from, to, transactions);
                case PeriodType.Month:
                    return GetMonthlyDataPoints(from, to, transactions);
                case PeriodType.Quarter:
                    return GetMonthlyDataPoints(from, to, transactions);
                case PeriodType.Year:
                    return GetMonthlyDataPoints(from, to, transactions);
                default:
                    throw new ArgumentOutOfRangeException(nameof(period), period, null);
            }
        }

        public IEnumerable<TransactionPeriodSummary> GetDailyDataPoints(DateTime from, DateTime to, IEnumerable<TransactionModel> transactions)
        {
            return CompileTransactionPeriodSummaries(from, to, t => t.AddDays(1), transactions);
        }

        public IEnumerable<TransactionPeriodSummary> GetWeeklyDataPoints(DateTime from, DateTime to, IEnumerable<TransactionModel> transactions)
        {
            var daysSinceMonday = ((int) from.DayOfWeek + 6) % 7;
            var startWeek = from.AddDays(-daysSinceMonday);
            return CompileTransactionPeriodSummaries(startWeek, to, t => t.AddDays(7), transactions);
        }

        public IEnumerable<TransactionPeriodSummary> GetMonthlyDataPoints(DateTime from, DateTime to, IEnumerable<TransactionModel> transactions)
        {
            var startMonth = new DateTime(from.Year, from.Month, 1);
            return CompileTransactionPeriodSummaries(startMonth, to, t => t.AddMonths(1), transactions);
        }

        public IEnumerable<TransactionPeriodSummary> GetQuarterlyDataPoints(DateTime from, DateTime to, IEnumerable<TransactionModel> transactions)
        {
            var startMonth = new DateTime(from.Year, from.Month - from.Month % 3, 1);
            return CompileTransactionPeriodSummaries(startMonth, to, t => t.AddMonths(3), transactions);
        }

        public IEnumerable<TransactionPeriodSummary> GetAnnualDataPoints(DateTime from, DateTime to, IEnumerable<TransactionModel> transactions)
        {
            var startMonth = new DateTime(from.Year, 1, 1);
            return CompileTransactionPeriodSummaries(startMonth, to, t => t.AddYears(1), transactions);
        }

        private static IEnumerable<TransactionPeriodSummary> CompileTransactionPeriodSummaries(
            DateTime from, DateTime to, Func<DateTime, DateTime> iterator, IEnumerable<TransactionModel> transactions)
        {
            var balances = new List<TransactionPeriodSummary>();
            var currentSummary = new TransactionPeriodSummary();
            var startBalance = 0;

            foreach (var transaction in transactions.OrderBy(t => t.Date).ThenBy(t => t.Ordinal))
            {
                for (var dt = from; dt <= transaction.Date; dt = iterator(dt))
                {

                    if (currentSummary.PeriodStart != dt)
                    {
                        currentSummary = new TransactionPeriodSummary
                        {
                            PeriodStart = dt
                        };
                        balances.Add(currentSummary);
                    }

                    currentSummary.EndBalance = transaction.Balance;
                    if (transaction.Date < dt) continue;
                    if (transaction.Amount > 0) currentSummary.TotalIncome += transaction.Amount;
                    if (transaction.Amount < 0) currentSummary.TotalSpending -= transaction.Amount;
                }
                from = transaction.Date;
            }

            for (var dt = from; dt <= to; dt = iterator(dt))
            {
                if (currentSummary.PeriodStart == dt) continue;

                currentSummary = new TransactionPeriodSummary
                {
                    PeriodStart = dt,
                    EndBalance = startBalance,
                    TotalIncome = 0,
                    TotalSpending = 0,
                };
                balances.Add(currentSummary);
            }

            return balances;
        }
    }
}