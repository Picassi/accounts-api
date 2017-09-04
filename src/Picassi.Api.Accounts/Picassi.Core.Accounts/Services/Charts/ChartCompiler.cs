using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Picassi.Api.Accounts.Contract.Transactions;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models;
using Picassi.Core.Accounts.Models.Accounts;
using Picassi.Core.Accounts.Models.Categories;
using Picassi.Core.Accounts.Models.Transactions;
using Picassi.Core.Accounts.Time.Periods;

namespace Picassi.Core.Accounts.Services.Charts
{
    public interface IChartCompiler
    {
        IEnumerable<PeriodSummaryDataSeries> GetTransactionSeriesData(DateTime from, DateTime to, PeriodType period, 
            GroupingType groupingType, int[] accounts = null, int[] categories = null);
        IList<DataSeriesModel> GetSpendingByCategoryDataSeries(DateTime from, DateTime to);
    }

    public class ChartCompiler : IChartCompiler
    {
        private readonly IAccountDataService _accountDataService;
        private readonly ICategoriesDataService _categoriesDataService;
        private readonly ITransactionsDataService _transactionsDataService;
        private readonly ITransactionPeriodSummariser _transactionPeriodSummariser;

        public ChartCompiler(IAccountDataService accountDataService, ICategoriesDataService categoriesDataService, ITransactionsDataService transactionsDataService, ITransactionPeriodSummariser transactionPeriodSummariser)
        {
            _accountDataService = accountDataService;
            _categoriesDataService = categoriesDataService;
            _transactionsDataService = transactionsDataService;
            _transactionPeriodSummariser = transactionPeriodSummariser;
        }


        public IEnumerable<PeriodSummaryDataSeries> GetTransactionSeriesData(DateTime from, DateTime to, PeriodType period, 
            GroupingType groupingType, int[] accounts = null, int[] categories = null)
        {
            var transactions = _transactionsDataService.Query(dateFrom: from, dateTo: to, pageSize: -1, 
                accounts: accounts?.Length > 0 ? accounts : null,
                categories: categories?.Length > 0 ? categories : null);
            var groups = GroupTransactions(groupingType, transactions, accounts, categories);

            return groups.Select(g => new PeriodSummaryDataSeries
            {
                Name = g.Key.ToString(),
                Data = _transactionPeriodSummariser.GetDataPoints(from, to, period, g).ToList()
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

        private IEnumerable<IGrouping<int?, TransactionModel>> GroupTransactions(
            GroupingType groupingType, IEnumerable<TransactionModel> transactions, int[] accountIds, int[] categoryIds)
        {
            switch (groupingType)
            {
                case GroupingType.Accounts:
                    var accounts = _accountDataService.Query(new AccountQueryModel { Ids = accountIds }).Select(a => a.Id);
                    return accounts.Select(a => new Grouping<int?, TransactionModel>(a, transactions.Where(t => t.AccountId == a)));
                case GroupingType.Categories:
                    var categories = _categoriesDataService.Query(new CategoriesQueryModel { Ids = categoryIds }).Select(c => c.Id);
                    return categories.Select(c => new Grouping<int?, TransactionModel>(c, transactions.Where(t => t.CategoryId == c)));
                default:
                    throw new InvalidEnumArgumentException();
            }
        }


        public class Grouping<TKey, TElement> : List<TElement>, IGrouping<TKey, TElement>
        {
            public Grouping(TKey key, IEnumerable<TElement> enumerable) : base(enumerable)
            {
                Key = key;                
            }

            public TKey Key
            {
                get;
                set;
            }
        }
    }
}
