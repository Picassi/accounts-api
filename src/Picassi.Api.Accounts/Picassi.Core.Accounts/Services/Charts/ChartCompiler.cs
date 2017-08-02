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
        IEnumerable<DataSeriesModel> GetAccountBalanceDataSeries(DateTime from, DateTime to);
    }

    public class ChartCompiler : IChartCompiler
    {
        private readonly ITransactionsDataService _transactionsDataService;

        public ChartCompiler(ITransactionsDataService transactionsDataService)
        {
            _transactionsDataService = transactionsDataService;
        }

        public IEnumerable<DataSeriesModel> GetAccountBalanceDataSeries(DateTime from, DateTime to)
        {
            var transactions = _transactionsDataService.Query(dateFrom: from, dateTo: to, pageSize: -1);

            return transactions.GroupBy(t => t.AccountId).Select(g => new DataSeriesModel
            {
                Name = g.Key.ToString(),
                Data = GetBalanceDataPoints(from, to, g.OrderBy(t => t.Date).ThenBy(t => t.Ordinal)).ToList()
            });
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
