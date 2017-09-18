using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
            var baseDate = DateTime.Today;
            var queryResults = DbProvider.GetDataContext().ModelledTransactions.Include("Category");
            var results = GetPeriodSummaries(queryResults, baseDate);
            var resultsWithDefaults = GetResultsWithDefaults(results, baseDate, results.Max(r => r.StartDate));
            AssignBalances(resultsWithDefaults);
            return resultsWithDefaults;
        }

        private List<TransactionCategoriesGroupedByPeriodModel> GetResultsWithDefaults(List<TransactionCategoriesGroupedByPeriodModel> results, DateTime start, DateTime end)
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

        private static List<TransactionCategoriesGroupedByPeriodModel> GetPeriodSummaries(IQueryable<ModelledTransaction> queryResults, DateTime baseDate)
        {
            var groupedByPeriod =
                queryResults.GroupBy(transaction => (int)DbFunctions.DiffDays(baseDate, transaction.Date) / 7);

            var results = groupedByPeriod.Select(grp => new TransactionCategoriesGroupedByPeriodModel
            {
                Description = "Week " + grp.Key,
                Credit = grp.Where(transaction => transaction.Amount > 0)
                    .Select(transaction => transaction.Amount)
                    .DefaultIfEmpty(0).Sum(),
                Debit = grp.Where(transaction => transaction.Amount < 0)
                    .Select(transaction => transaction.Amount)
                    .DefaultIfEmpty(0).Sum(),
                StartDate = (DateTime)DbFunctions.AddDays(baseDate, grp.Key * 7),
                Transactions = grp.GroupBy(transactions => new { transactions.CategoryId, transactions.Category.Name })
                    .Select(grp2 => new TransactionsGroupedByCategoryModel
                        {
                            Description = grp2.Key.Name ?? "Uncategorised",
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

        private static void AssignBalances(List<TransactionCategoriesGroupedByPeriodModel> results)
        {
            decimal balance = 0;
            foreach (var result in results)
            {
                balance += result.Amount;
                result.Balance = balance;
            }
        }
    }
}
