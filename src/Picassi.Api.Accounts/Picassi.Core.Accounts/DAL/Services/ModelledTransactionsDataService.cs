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
        IList<TransactionCategoriesGroupedByPeriodModel> QueryWeekly(int accountId, ModelledTransactionQueryModel query);
    }

    public class ModelledTransactionsDataService : GenericDataService<ModelledTransactionModel, ModelledTransaction>, IModelledTransactionsDataService
    {
        private readonly IModelMapper<ModelledTransactionModel, ModelledTransaction> _modelMapper;
        public ModelledTransactionsDataService(IModelMapper<ModelledTransactionModel, ModelledTransaction> modelMapper, IAccountsDataContext dbContext) 
            : base(modelMapper, dbContext)
        {
            _modelMapper = modelMapper;
        }

        public ResultsViewModel<ModelledTransactionModel> Query(int accountId, ModelledTransactionQueryModel query)
        {
            var queryResults = GetBaseTransactions(accountId);
            var orderedResults = queryResults.OrderBy(t => t.Date).ThenBy(t => t.Ordinal).AsQueryable();
            var allResults = queryResults.Count();

            var actualResults = PageResults(query.PageNumber, query.PageSize, orderedResults);

            return new ResultsViewModel<ModelledTransactionModel>
            {
                TotalLines = allResults,
                Lines = actualResults.Select(_modelMapper.Map).ToList()
            };
        }

        public IList<TransactionCategoriesGroupedByPeriodModel> QueryWeekly(int accountId, ModelledTransactionQueryModel query)
        {
            var baseDate = DateTime.Today;
            var queryResults = GetBaseTransactions(accountId);
            var groupedByPeriod = queryResults.GroupBy(transaction => (int)DbFunctions.DiffDays(baseDate, transaction.Date) / 7);

            return groupedByPeriod.Select(grp => new TransactionCategoriesGroupedByPeriodModel
            {
                Description = "Week " + grp.Key,
                Balance = 0,
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
        }

        private IQueryable<ModelledTransaction> GetBaseTransactions(int accountId)
        {
            var queryResults = DbContext.ModelledTransactions.Include("Category")
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

    }
}
