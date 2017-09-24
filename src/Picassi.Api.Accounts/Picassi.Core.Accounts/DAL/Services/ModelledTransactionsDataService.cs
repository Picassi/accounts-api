using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Picassi.Api.Accounts.Contract;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Models;
using Picassi.Core.Accounts.Models.ModelledTransactions;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.DAL.Services
{
    public interface IModelledTransactionsDataService : IGenericDataService<ModelledTransactionModel>
    {
        ResultsViewModel<ModelledTransactionModel> Query(int accountId, ModelledTransactionQueryModel query);
        IList<TransactionCategoriesGroupedByPeriodModel> QueryGrouped(ModelledTransactionQueryModel query);
    }

    public class ModelledTransactionsDataService : GenericDataService<ModelledTransactionModel, ModelledTransaction>, IModelledTransactionsDataService
    {
        private readonly IModelMapper<ModelledTransactionModel, ModelledTransaction> _modelMapper;

        public ModelledTransactionsDataService(IModelMapper<ModelledTransactionModel, ModelledTransaction> modelMapper, IAccountsDatabaseProvider dbProvider) 
            : base(modelMapper, dbProvider)
        {
            _modelMapper = modelMapper;
        }

        public ResultsViewModel<ModelledTransactionModel> Query(int accountId, ModelledTransactionQueryModel query)
        {
            var queryResults = GetBaseTransactions(accountId);
            var orderedResults = queryResults.OrderBy(t => t.Date).ThenBy(t => t.Ordinal).AsQueryable();
            var allResults = queryResults.Count();

            var actualResults = PageResults(query.PageNumber, query.PageSize, orderedResults).ToList();

            return new ResultsViewModel<ModelledTransactionModel>
            {
                TotalLines = allResults,
                Lines = actualResults.Select(_modelMapper.Map).ToList()
            };
        }

        public IList<TransactionCategoriesGroupedByPeriodModel> QueryGrouped(ModelledTransactionQueryModel query)
        {
            var modelledResults = GetModelledTransactions(query);
            var recordedResults = GetRecordedTansactions(query);
            var queryResults = modelledResults.Union(recordedResults);

            var results = GetPeriodSummaries(queryResults, query.DateFrom);
            var resultsWithDefaults = GetResultsWithDefaults(results, query.DateFrom, results.Select(r => r.StartDate).DefaultIfEmpty(query.DateTo).Max());

            var startingBalance = GetBalance(query.DateFrom);
            AssignBalances(startingBalance, resultsWithDefaults);
            return resultsWithDefaults;
        }

        private IQueryable<ProjectedTransaction> GetModelledTransactions(ModelledTransactionQueryModel query)
        {
            var modelledResults = DbProvider.GetDataContext().ModelledTransactions
                .Include("Category")
                .Where(x => x.Date >= query.DateFrom && x.Date <= query.DateTo)
                .Select(t => new ProjectedTransaction
                {
                    Amount = t.Amount,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category == null ? null : t.Category.Name,
                    Date = t.Date
                });
            return modelledResults;
        }

        private IQueryable<ProjectedTransaction> GetRecordedTansactions(ModelledTransactionQueryModel query)
        {
            var recordedResults = DbProvider.GetDataContext().Transactions
                .Include("Category")
                .Where(x => x.Date >= query.DateFrom && x.Date <= query.DateTo)
                .Select(t => new ProjectedTransaction
                {
                    Amount = t.Amount,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category == null ? null : t.Category.Name,
                    Date = t.Date
                });
            return recordedResults;
        }

        private List<TransactionCategoriesGroupedByPeriodModel> GetResultsWithDefaults(IList<TransactionCategoriesGroupedByPeriodModel> results, DateTime start, DateTime end)
        {
            var resultsToReturn = new List<TransactionCategoriesGroupedByPeriodModel>();
            var week = 0;
            for (var date = start; date <= end; date = date.AddDays(7))
            {
                week++;
                var groupedModel = results.FirstOrDefault(r => r.StartDate == date) ?? new TransactionCategoriesGroupedByPeriodModel
                {
                    Description = "Week " + week,
                    Credit = 0,
                    Debit = 0,
                    StartDate = date,
                    Transactions = new List<TransactionsGroupedByCategoryModel>()
                };
                resultsToReturn.Add(groupedModel);
            }
            return resultsToReturn;
        }

        private IQueryable<ModelledTransaction> GetBaseTransactions(int accountId)
        {
            var queryResults = DbProvider.GetDataContext().ModelledTransactions.Include("Category")
                .Where(t => t.AccountId == accountId);
            return queryResults;
        }

        private static IQueryable<T> PageResults<T>(int? pageNumber, int? size, IQueryable<T> queryResults)
        {
            var pageSize = size ?? 20;
            var skip = ((pageNumber ?? 1) - 1) * pageSize;
            var actualResults = queryResults.Skip(skip).Take(pageSize);
            return actualResults;
        }

        private static List<TransactionCategoriesGroupedByPeriodModel> GetPeriodSummaries(IQueryable<ProjectedTransaction> queryResults, DateTime baseDate)
        {
            var groupedByPeriod =
                queryResults.GroupBy(transaction => (int)DbFunctions.DiffDays(baseDate, transaction.Date) / 7);

            var results = groupedByPeriod.Select(grp => new TransactionCategoriesGroupedByPeriodModel
            {
                Description = "Week " + (grp.Key + 1),
                Credit = grp.Where(transaction => transaction.Amount > 0)
                    .Select(transaction => transaction.Amount)
                    .DefaultIfEmpty(0).Sum(),
                Debit = grp.Where(transaction => transaction.Amount < 0)
                    .Select(transaction => transaction.Amount)
                    .DefaultIfEmpty(0).Sum(),
                StartDate = (DateTime)DbFunctions.AddDays(baseDate, grp.Key * 7),
                Transactions = grp.GroupBy(transactions => new { transactions.CategoryId, transactions.CategoryName })
                    .Select(grp2 => new TransactionsGroupedByCategoryModel
                        {
                            Description = grp2.Key.CategoryName ?? "Uncategorised",
                            CategoryId = grp2.Key.CategoryId,
                            Count = grp2.Count(),
                            Credit = grp2.Where(transaction => transaction.Amount > 0)
                                .Select(transaction => transaction.Amount)
                                .DefaultIfEmpty(0).Sum(),
                            Debit = grp2.Where(transaction => transaction.Amount < 0)
                                .Select(transaction => transaction.Amount)
                                .DefaultIfEmpty(0).Sum()
                        }
                    )
            }).OrderBy(x => x.StartDate).ToList();
            return results;
        }

        private static void AssignBalances(decimal startingBalance, List<TransactionCategoriesGroupedByPeriodModel> results)
        {
            foreach (var result in results)
            {
                startingBalance += result.Amount;
                result.Balance = startingBalance;
            }
        }

        private decimal GetBalance(DateTime date)
        {
            var mostRecentTransaction = DbProvider.GetDataContext().Transactions.Select(t => new {  t.Date, t.Balance, t.Ordinal })
                .Union(DbProvider.GetDataContext().ModelledTransactions.Select(m => new { m.Date, m.Balance, m.Ordinal}))
                .Where(t => t.Date < date)
                .OrderByDescending(t => t.Date)
                .ThenByDescending(t => t.Ordinal)
                .FirstOrDefault();

            return mostRecentTransaction?.Balance ?? 0;
        }
    }

    public class ProjectedTransaction
    {
        public DateTime Date { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Amount { get; set; }
    }
}
