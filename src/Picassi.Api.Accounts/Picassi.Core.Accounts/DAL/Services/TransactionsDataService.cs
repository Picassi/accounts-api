using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Models.Transactions;
using Picassi.Core.Accounts.Services;
using Picassi.Utils.Data.Extensions;

namespace Picassi.Core.Accounts.DAL.Services
{
    public interface ITransactionsDataService : IGenericDataService<TransactionModel>
    {
        IEnumerable<TransactionModel> Query(SimpleTransactionQueryModel query);
        TransactionsResultsViewModel Query(TransactionsQueryModel query);
    }

    public class TransactionsDataService : GenericDataService<TransactionModel, Transaction>, ITransactionsDataService
    {
        public TransactionsDataService(ITransactionModelMapper modelMapper, IAccountsDataContext dbContext) 
            : base(modelMapper, dbContext)
        {
        }

        public IEnumerable<TransactionModel> Query(SimpleTransactionQueryModel query)
        {
            var transactions = DbContext.Transactions.Where(x => x.AccountId == query.AccountId);
            if (query.DateFrom != null) transactions = transactions.Where(x => x.Date >= query.DateFrom);
            if (query.DateTo != null) transactions = transactions.Where(x => x.Date < query.DateTo);
            return Mapper.Map<IEnumerable<TransactionModel>>(transactions);
        }

        public TransactionsResultsViewModel Query(TransactionsQueryModel query)
        {
            var transactions = DbContext.Transactions.Include(x => x.Category);
            var results = (query == null ? transactions : FilterTransactions(query, transactions)).ToList();
            var count = query == null ? results.Count : FilterTransactionsWithoutPaging(query, transactions).Count();

            return new TransactionsResultsViewModel
            {
                TotalLines = count,
                Total = results.Sum(x => x.Amount),
                Lines = Mapper.Map<IEnumerable<TransactionModel>>(results)
            };
        }

        private static IEnumerable<Transaction> FilterTransactions(TransactionsQueryModel query, IQueryable<Transaction> transactions)
        {
            transactions = FilterTransactionsWithoutPaging(query, transactions);
            transactions = OrderResults(transactions, query.SortBy, query.SortAscending);
            transactions = PageResults(transactions, query.PageNumber, query.PageSize);
            return transactions;
        }

        private static IQueryable<Transaction> FilterTransactionsWithoutPaging(TransactionsQueryModel query, IQueryable<Transaction> transactions)
        {
            transactions = FilterText(transactions, query.Text);
            transactions = FilterAccounts(transactions, query.Accounts);
            transactions = FilterCategories(transactions, query.ShowAllCategorised, query.ShowUncategorised, query.Categories);
            transactions = FilterDate(transactions, query.DateFrom, query.DateTo);
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

        private static IQueryable<Transaction> FilterCategories(IQueryable<Transaction> transactions, bool showAllCategories, bool showUncategorised, int[] categoryIds)
        {
            if (categoryIds == null) categoryIds = new int[0];
            return transactions.Where(x =>
                (showUncategorised && x.CategoryId == null) ||
                (showAllCategories && x.CategoryId != null) ||
                (x.CategoryId != null && categoryIds.Contains((int)x.CategoryId)));

        }

        private static IQueryable<Transaction> PageResults(IQueryable<Transaction> transactions, int? pageNumber, int? pageSize)
        {
            return transactions.Skip(((pageNumber ?? 1) - 1) * (pageSize ?? 20)).Take(pageSize ?? 20);
        }

        private static IQueryable<Transaction> OrderResults(IQueryable<Transaction> transactions, string field, bool ascending)
        {
            if (field == null)
            {
                field = "Date";
                ascending = true;
            }

            return transactions.OrderBy(field, @ascending).ThenBy("Ordinal", @ascending);
        }
    }
}
