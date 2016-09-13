using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using Picassi.Core.Accounts.ViewModels.Transactions;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;
using Picassi.Common.Data.Extensions;
using Picassi.Core.Accounts.ViewModels;

namespace Picassi.Core.Accounts.DbAccess.Transactions
{
    public interface ITransactionQueryService
    {
        IEnumerable<TransactionViewModel> Query(SimpleTransactionQueryModel query);
        TransactionsResultsViewModel Query(TransactionsQueryModel query);
        ResultsViewModel<AccountTransactionViewModel> Query(int id, AccountTransactionsQueryModel query);
    }

    public class TransactionQueryService : ITransactionQueryService
    {
        private readonly IAccountsDataContext _dbContext;
        
        public TransactionQueryService(IAccountsDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        public IEnumerable<TransactionViewModel> Query(SimpleTransactionQueryModel query)
        {
            var transactions = _dbContext.Transactions.Where(x => x.FromId == query.AccountId || x.ToId == query.AccountId);
            if (query.DateFrom != null) transactions = transactions.Where(x => x.Date >= query.DateFrom);
            if (query.DateTo != null) transactions = transactions.Where(x => x.Date < query.DateTo);
            return Mapper.Map<IEnumerable<TransactionViewModel>>(transactions);
        }

        public TransactionsResultsViewModel Query(TransactionsQueryModel query)
        {
            var transactions = _dbContext.Transactions.Include(x => x.Category);
            var results = (query == null ? transactions : FilterTransactions(query, transactions)).ToList();
            var count = query == null ? results.Count : FilterTransactionsWithoutPaging(query, transactions).Count();

            return new TransactionsResultsViewModel
            {
                TotalLines = count,
                Total = results.Sum(x => x.Amount),
                Lines = Mapper.Map<IEnumerable<TransactionViewModel>>(results)
            };
        }

        public ResultsViewModel<AccountTransactionViewModel> Query(int accountId, AccountTransactionsQueryModel query)
        {
            var transactions = _dbContext.Transactions.Include(x => x.Category);
            var results = (query == null ? transactions : FilterTransactions(accountId, query, transactions)).ToList();
            var count = query == null ? results.Count : FilterTransactionsWithoutPaging(accountId, query, transactions).Count();

            return new ResultsViewModel<AccountTransactionViewModel>
            {
                TotalLines = count,
                Lines = GetAccountLines(accountId, results.ToList())
            };
        }

        private static IEnumerable<Transaction> FilterTransactions(TransactionsQueryModel query, IQueryable<Transaction> transactions)
        {
            transactions = FilterTransactionsWithoutPaging(query, transactions);
            transactions = OrderResults(transactions, query.SortBy, query.SortAscending);
            transactions = PageResults(transactions, query.PageNumber, query.PageSize);
            return transactions;
        }

        private static IEnumerable<Transaction> FilterTransactions(int accountId, AccountTransactionsQueryModel query, IQueryable<Transaction> transactions)
        {
            transactions = FilterTransactionsWithoutPaging(accountId, query, transactions);
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
            transactions = FilterProvisional(transactions, query.IncludeProvisional, query.IncludeConfirmed);
            return transactions;
        }

        private static IQueryable<Transaction> FilterTransactionsWithoutPaging(int accountId, AccountTransactionsQueryModel query, IQueryable<Transaction> transactions)
        {
            transactions = FilterText(transactions, query.Text);
            transactions = FilterAccounts(transactions, accountId);
            transactions = FilterCategories(transactions, query.ShowAllCategorised, query.ShowUncategorised, query.Categories);
            transactions = FilterDate(transactions, query.DateFrom, query.DateTo);
            transactions = FilterProvisional(transactions, query.IncludeProvisional, query.IncludeConfirmed);
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
            return accountIds == null || accountIds.Length == 0 ? transactions : 
                transactions.Where(x => 
                (x.FromId != null && accountIds.Contains((int)x.FromId)) || 
                (x.ToId != null && accountIds.Contains((int)x.ToId)));
        }

        private static IQueryable<Transaction> FilterAccounts(IQueryable<Transaction> transactions, int accountId)
        {
            return transactions.Where(x => x.FromId == accountId || x.ToId == accountId);
        }

        private static IQueryable<Transaction> FilterCategories(IQueryable<Transaction> transactions, bool showAllCategories, bool showUncategorised, int[] categoryIds)
        {
            if (categoryIds == null) categoryIds = new int[0];
            return transactions.Where(x =>
                (showUncategorised && x.CategoryId == null) ||
                (showAllCategories && x.CategoryId != null) ||
                (x.CategoryId != null && categoryIds.Contains((int) x.CategoryId)));

        }

        private static IQueryable<Transaction> FilterProvisional(IQueryable<Transaction> transactions, bool? includeProvisional, bool? includeConfirmed)
        {
            var provisional = includeProvisional == true;
            var confirmed = includeConfirmed == true;

            if (provisional && confirmed) return transactions;

            return transactions.Where(x => provisional && x.Status == TransactionStatus.Provisional || (confirmed && x.Status == TransactionStatus.Confirmed));
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

        private static IEnumerable<AccountTransactionViewModel> GetAccountLines(int accountId, IEnumerable<Transaction> transactions)
        {
            return transactions.Select(transaction => new AccountTransactionViewModel
            {
                Id = transaction.Id,
                Description = transaction.Description,
                AccountId = accountId,
                LinkedAccountId = transaction.FromId == accountId ? transaction.ToId : transaction.FromId,
                CategoryId = transaction.CategoryId,
                CategoryName = transaction.Category?.Name,
                Amount = transaction.FromId == accountId ? -transaction.Amount : transaction.Amount,
                Balance = transaction.Balance,
                Date = transaction.Date,
                Status = transaction.Status.ToString(),
            }).ToList();
        }
    }
}
