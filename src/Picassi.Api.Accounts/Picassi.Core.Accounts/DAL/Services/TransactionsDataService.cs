using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Picassi.Api.Accounts.Contract.Transactions;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Exceptions;
using Picassi.Core.Accounts.Models.Transactions;
using Picassi.Core.Accounts.Services;
using Picassi.Core.Accounts.Extensions;

namespace Picassi.Core.Accounts.DAL.Services
{
    public interface ITransactionsDataService : IGenericDataService<TransactionModel>
    {
        IEnumerable<TransactionModel> Query(
            string text = null,
            int[] accounts = null,
            int[] categories = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            bool? showUncategorised = true,
            bool? showAllCategories = null,
            int? pageSize = null,
            int? pageNumber = null,
            string sortBy = null,
            bool? sortAscending = true);

        TransactionsResultsViewModel QueryWithCount(
            string text = null, 
            int[] accounts = null, 
            int[] categories = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            bool? showUncategorised = null,
            bool? showAllCategories = null,
            int? pageSize = null,
            int? pageNumber = null,
            string sortBy = null,
            bool? sortAscending = null);
    }

    public class TransactionsDataService : GenericDataService<TransactionModel, Transaction>, ITransactionsDataService
    {
        public TransactionsDataService(ITransactionModelMapper modelMapper, IAccountsDatabaseProvider dbProvider) 
            : base(modelMapper, dbProvider)
        {
        }

        public override TransactionModel Create(TransactionModel model)
        {
            var dataModel = ModelMapper.CreateEntity(model);
            DbProvider.GetDataContext().Transactions.Add(dataModel);
            DbProvider.GetDataContext().SaveChanges();
            return Get(dataModel.Id);
        }

        public override TransactionModel Get(int id)
        {
            var entity = DbProvider.GetDataContext().Transactions
                .Include("Account")
                .Include("Category")
                .SingleOrDefault(x => x.Id == id);
            if (entity == null) throw new EntityNotFoundException<Transaction>(id);

            return ModelMapper.Map(entity);
        }

        public IEnumerable<TransactionModel> Query(
            string text = null,
            int[] accounts = null,
            int[] categories = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            bool? showUncategorised = null,
            bool? showAllCategories = null,
            int? pageSize = null,
            int? pageNumber = null,
            string sortBy = null,
            bool? sortAscending = null)
        {
            var transactions = DbProvider.GetDataContext().Transactions.Include(x => x.Category);
            var results = FilterTransactions(text, accounts, categories, dateFrom, dateTo, showUncategorised, 
                showAllCategories, pageSize, pageNumber, sortBy, sortAscending == true, transactions).ToList();

            return ModelMapper.MapList(results);
        }

        public TransactionsResultsViewModel QueryWithCount(
            string text = null,
            int[] accounts = null,
            int[] categories = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            bool? showUncategorised = null,
            bool? showAllCategories = null,
            int? pageSize = null,
            int? pageNumber = null,
            string sortBy = null,
            bool? sortAscending = null)
        {
            var transactions = DbProvider.GetDataContext().Transactions.Include(x => x.Category).Include(x => x.Account);
            var results = FilterTransactions(text, accounts, categories, dateFrom, dateTo, showUncategorised, 
                showAllCategories, pageSize, pageNumber, sortBy, sortAscending != false, transactions).ToList();
            var count = FilterTransactionsWithoutPaging(text, accounts, categories, dateFrom, dateTo, 
                showUncategorised != false, showAllCategories ?? categories == null, transactions).Count();

            return new TransactionsResultsViewModel
            {
                TotalLines = count,
                Total = results.Sum(x => x.Amount),
                Lines = ModelMapper.MapList(results)
            };
        }

        private static IEnumerable<Transaction> FilterTransactions(string text, int[] accounts, int[] categories,
            DateTime? dateFrom, DateTime? dateTo, bool? showUncategorised, bool? showAllCategories, int? pageSize, int? pageNumber,
            string sortBy, bool sortAscending, IQueryable <Transaction> transactions)
        {
            transactions = FilterTransactionsWithoutPaging(text, accounts, categories, dateFrom, dateTo, 
                ShouldShouldUncategorised(showUncategorised, categories), showAllCategories ?? categories == null, transactions);
            transactions = OrderResults(transactions, sortBy, sortAscending);
            transactions = PageResults(transactions, pageNumber, pageSize);
            return transactions;
        }

        private static IQueryable<Transaction> FilterTransactionsWithoutPaging(string text, int[] accounts, int[] categories,
            DateTime? dateFrom, DateTime? dateTo, bool showUncategorised, bool showAllCategories, IQueryable<Transaction> transactions)
        {
            transactions = FilterText(transactions, text);
            transactions = FilterAccounts(transactions, accounts);
            transactions = FilterCategories(transactions, categories, showUncategorised, showAllCategories);
            transactions = FilterDate(transactions, dateFrom, dateTo);
            return transactions;
        }

        private static IQueryable<Transaction> FilterText(IQueryable<Transaction> transactions, string text)
        {
            return text == null ? transactions : transactions.Where(x => x.Description != null && x.Description.Contains(text));
        }

        private static IQueryable<Transaction> FilterDate(IQueryable<Transaction> transactions, DateTime? start, DateTime? end)
        {
            return transactions.Where(x => (start == null || x.Date >= start) && (end == null || x.Date < end));
        }

        private static IQueryable<Transaction> FilterAccounts(IQueryable<Transaction> transactions, int[] accountIds)
        {
            return accountIds == null || accountIds.Length == 0
                ? transactions
                : transactions.Where(x => accountIds.Contains((int) x.AccountId));
        }

        private static IQueryable<Transaction> FilterCategories(IQueryable<Transaction> transactions, int[] categoryIds, bool showUncategorised, bool showAllCategories)
        {
            if (categoryIds == null) categoryIds = new int[0];
            return transactions.Where(x =>
                (showUncategorised && x.CategoryId == null) ||
                (showAllCategories && x.CategoryId != null) ||
                (x.CategoryId != null && categoryIds.Contains((int)x.CategoryId)));

        }

        private static IQueryable<Transaction> PageResults(IQueryable<Transaction> transactions, int? pageNumber, int? pageSize)
        {
            return pageSize < 0 ? transactions : transactions.Skip(((pageNumber ?? 1) - 1) * (pageSize ?? 20)).Take(pageSize ?? 20);
        }

        private static IQueryable<Transaction> OrderResults(IQueryable<Transaction> transactions, string field, bool ascending)
        {
            if (field == null)
            {
                field = "Date";
                ascending = false;
            }

            return transactions.OrderBy(field, @ascending).ThenBy("Ordinal", @ascending);
        }

        private static bool ShouldShouldUncategorised(bool? showUncategorised, int[] categories)
        {
            return (showUncategorised == null && categories == null) || showUncategorised == true;
        }

    }
}
