using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Picassi.Api.Accounts.Contract;
using Picassi.Api.Accounts.Contract.Calendar;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.DAL.Transforms;
using Picassi.Core.Accounts.Models.ModelledTransactions;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.DAL.Services
{
    public interface IModelledTransactionsDataService : IGenericDataService<ModelledTransactionModel>
    {
        ResultsViewModel<ModelledTransactionModel> Query(
            string text = null,
            int[] accounts = null,
            int[] categories = null,
            int[] budgets = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            bool? showUncategorised = true,
            bool? showAllCategories = null,
            bool? includeSubcategories = true,
            int? pageSize = null,
            int? pageNumber = null,
            string sortBy = null,
            bool? sortAscending = true);

        IList<TransactionCategoriesGroupedByPeriodModel> QueryGrouped(
            DateTime dateFrom,
            DateTime dateTo,
            ReportingPeriod period,
            string text = null,
            int[] accounts = null,
            int[] categories = null,
            int[] budgets = null,
            bool? showUncategorised = true,
            bool? showAllCategories = null,
            bool? includeSubcategories = true,
            int? pageSize = null,
            int? pageNumber = null,
            string sortBy = null,
            bool? sortAscending = true);
    }

    public class ModelledTransactionsDataService : GenericDataService<ModelledTransactionModel, ModelledTransaction>, IModelledTransactionsDataService
    {
        private readonly IModelMapper<ModelledTransactionModel, ModelledTransaction> _modelMapper;
        private readonly IEnumerable<ITransactionCategoriesByPeriodTransform> _transactionCategoryPeriodTransforms;

        public ModelledTransactionsDataService(
            IModelMapper<ModelledTransactionModel, ModelledTransaction> modelMapper, 
            IAccountsDatabaseProvider dbProvider,
            IEnumerable<ITransactionCategoriesByPeriodTransform> transactionCategoryPeriodTransforms) 
            : base(modelMapper, dbProvider)
        {
            _modelMapper = modelMapper;
            _transactionCategoryPeriodTransforms = transactionCategoryPeriodTransforms;
        }

        public ResultsViewModel<ModelledTransactionModel> Query(
            string text = null,
            int[] accounts = null,
            int[] categories = null,
            int[] budgets = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            bool? showUncategorised = true,
            bool? showAllCategories = null,
            bool? includeSubcategories = true,
            int? pageSize = null,
            int? pageNumber = null,
            string sortBy = null,
            bool? sortAscending = true)
        {
            var queryResults = GetBaseTransactions();

            if (dateFrom != null)
            {
                queryResults = queryResults.Where(t => t.Date >= dateFrom);
            }

            if (dateTo != null)
            {
                queryResults = queryResults.Where(t => t.Date < dateTo);
            }

            if (accounts != null && accounts.Length > 0)
            {
                queryResults = queryResults.Where(t => accounts.Contains(t.AccountId));
            }

            if (budgets != null && budgets.Length > 0)
            {
                queryResults = queryResults.Where(t => t.BudgetId != null && budgets.Contains((int)t.BudgetId));
            }

            var orderedResults = queryResults.OrderBy(t => t.Date).ThenBy(t => t.Ordinal).AsQueryable();
            var allResults = queryResults.Count();

            if (pageNumber != null && pageSize != null)
            {
                orderedResults = PageResults((int)pageNumber, (int)pageSize, orderedResults);
            }

            var actualResults = orderedResults.ToList();

            return new ResultsViewModel<ModelledTransactionModel>
            {
                TotalLines = allResults,
                Lines = actualResults.Select(_modelMapper.Map).ToList()
            };
        }

        public IList<TransactionCategoriesGroupedByPeriodModel> QueryGrouped(
            DateTime dateFrom,
            DateTime dateTo,
            ReportingPeriod period,
            string text = null,
            int[] accounts = null,
            int[] categories = null,
            int[] budgets = null,
            bool? showUncategorised = true,
            bool? showAllCategories = null,
            bool? includeSubcategories = true,
            int? pageSize = null,
            int? pageNumber = null,
            string sortBy = null,
            bool? sortAscending = true)
        {
            var modelledResults = GetModelledTransactions(dateFrom, dateTo);
            var recordedResults = GetRecordedTansactions(dateFrom, dateTo);
            var queryResults = modelledResults.Union(recordedResults);

            var resultsWithDefaults = GetPeriodSummaries(dateFrom, dateTo, period, queryResults);

            var startingBalance = GetBalance(dateFrom);
            AssignBalances(startingBalance, resultsWithDefaults);
            return resultsWithDefaults;
        }

        private IQueryable<ProjectedTransaction> GetModelledTransactions(DateTime dateFrom, DateTime dateTo)
        {
            var modelledResults = DbProvider.GetDataContext().ModelledTransactions
                .Include("Category")
                .Where(x => x.Date >= dateFrom && x.Date < dateTo)
                .Select(t => new ProjectedTransaction
                {
                    Amount = t.Amount,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category == null ? null : t.Category.Name,
                    Date = t.Date
                });
            return modelledResults;
        }

        private IQueryable<ProjectedTransaction> GetRecordedTansactions(DateTime dateFrom, DateTime dateTo)
        {
            var recordedResults = DbProvider.GetDataContext().Transactions
                .Include("Category")
                .Where(x => x.Date >= dateFrom && x.Date < dateTo)
                .Select(t => new ProjectedTransaction
                {
                    Amount = t.Amount,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category == null ? null : t.Category.Name,
                    Date = t.Date
                });
            return recordedResults;
        }

        private IQueryable<ModelledTransaction> GetBaseTransactions()
        {
            return DbProvider.GetDataContext().ModelledTransactions.Include("Category");                
        }

        private static IQueryable<T> PageResults<T>(int pageNumber, int pageSize, IQueryable<T> queryResults)
        {
            var skip = (pageNumber - 1) * pageSize;
            var actualResults = queryResults.Skip(skip).Take(pageSize);
            return actualResults;
        }

        private List<TransactionCategoriesGroupedByPeriodModel> GetPeriodSummaries(DateTime start, DateTime end, ReportingPeriod period, IQueryable<ProjectedTransaction> queryResults)
        {
            var transform = _transactionCategoryPeriodTransforms.Single(t => t.Period == period);
            return transform.GetPeriodSummaries(queryResults, start, end);
        }

        private static void AssignBalances(decimal startingBalance, List<TransactionCategoriesGroupedByPeriodModel> results)
        {
            foreach (var result in results)
            {
                result.StartBalance = startingBalance;
                startingBalance += result.Amount;
                result.EndBalance = startingBalance;
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

