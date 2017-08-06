using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models;
using Picassi.Core.Accounts.Models.Transactions;

namespace Picassi.Core.Accounts.Services.Charts
{
    public interface IChartCompiler
    {
        IEnumerable<ImplicitDataSeriesModel> GetAccountBalanceDataSeries(DateTime from, DateTime to);
        IList<DataSeriesModel> GetSpendingByCategoryDataSeries(DateTime from, DateTime to);
    }

    public class ChartCompiler : IChartCompiler
    {
        private readonly ITransactionsDataService _transactionsDataService;

        public ChartCompiler(ITransactionsDataService transactionsDataService)
        {
            _transactionsDataService = transactionsDataService;
        }

        public IEnumerable<ImplicitDataSeriesModel> GetAccountBalanceDataSeries(DateTime from, DateTime to)
        {
            var transactions = _transactionsDataService.Query(dateFrom: from, dateTo: to, pageSize: -1);

            return transactions.GroupBy(t => t.AccountId).Select(g => new ImplicitDataSeriesModel
            {
                Name = g.Key.ToString(),
                Data = GetBalanceDataPoints(from, to, g.OrderBy(t => t.Date).ThenBy(t => t.Ordinal)).ToList()
            });
        }

        public IList<DataSeriesModel> GetSpendingByCategoryDataSeries(DateTime from, DateTime to)
        {
            var transactions = _transactionsDataService.Query(dateFrom: from, dateTo: to, pageSize: -1);

            var dataPoints = transactions.GroupBy(t => t.CategoryName).Select(g => new 
            {
                Name = g.Key,
                Spending = Math.Abs(g.Sum(t => t.Amount))
            }).ToList();

            var spendingSeries = new DataSeriesModel
            {
                Name = "Spending by category",
                ColorByPoint = true,
                Data = dataPoints.Where(d => d.Spending <= 0).Select(a => new DataPointModel
                {
                    Name = a.Name,
                    Y = Math.Abs(a.Spending)
                }).ToList()
            };

            var incomeSeries = new DataSeriesModel
            {
                Name = "Income by category",
                ColorByPoint = true,
                Data = dataPoints.Where(d => d.Spending > 0).Select(a => new DataPointModel
                {
                    Name = a.Name,
                    Y = Math.Abs(a.Spending)
                }).ToList()
            };
            return new[] {spendingSeries, incomeSeries};
        }

        private static IEnumerable<object[]> GetBalanceDataPoints(DateTime from, DateTime to, IOrderedEnumerable<TransactionModel> transactions)
        {
            var balances = new Dictionary<DateTime, decimal>();
            var lastRecorded = from;

            foreach (var transaction in transactions)
            {
                for (var dt = lastRecorded; dt <= transaction.Date; dt = dt.AddDays(1))
                {
                    balances[dt] = transaction.Balance;
                }
                lastRecorded = transaction.Date;
            }
            return balances.Select(b => new object[] { b.Key.ToString("dd/MM/yyyy"), b.Value });
        }
    }
}
