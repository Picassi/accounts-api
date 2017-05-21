using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Events;
using Picassi.Core.Accounts.Events.Messages;
using Picassi.Core.Accounts.Models;
using Picassi.Core.Accounts.Models.Transactions;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.DAL.Services
{
    public interface IAccountTransactionsDataService : IGenericDataService<AccountTransactionModel>
    {
        ResultsViewModel<AccountTransactionModel> Query(int accountId, AccountTransactionsQueryModel query);
        void InsertTransactionAt(int id, int position);
        void MoveTransactionUp(int accountId, int transactionId);
        void MoveTransactionDown(int accountId, int transactionId);
    }

    public class AccountTransactionsDataService : GenericDataService<AccountTransactionModel, Transaction>, IAccountTransactionsDataService
    {
        private readonly ITransactionsDataService _transactionsDataService;

        public AccountTransactionsDataService(
            IAccountTransactionModelMapper modelMapper, 
            IAccountsDataContext dbContext, ITransactionsDataService transactionsDataService) 
            : base(modelMapper, dbContext)
        {
            _transactionsDataService = transactionsDataService;
        }

        public ResultsViewModel<AccountTransactionModel> Query(int accountId, AccountTransactionsQueryModel query)
        {
            var transactionsQuery = new TransactionsQueryModel
            {
                PageSize = query.PageSize,
                PageNumber = query.PageSize, 
                Text = query.Text,
                Accounts = new [] { accountId },
                Categories = query.Categories,
                ShowAllCategorised = query.ShowAllCategorised,
                ShowUncategorised = query.ShowUncategorised,
                DateFrom = query.DateFrom,
                DateTo = query.DateTo,
                SortBy = query.SortBy,
                SortAscending = query.SortAscending
            };

            var transactions = _transactionsDataService.Query(transactionsQuery);

            return new ResultsViewModel<AccountTransactionModel>
            {
                TotalLines = transactions.TotalLines,
                Lines = GetAccountLines(accountId, transactions.Lines)
            };
        }

        public void InsertTransactionAt(int id, int position)
        {
            DbContext.Database.ExecuteSqlCommand($"Update accounts.transactions set ordinal = ordinal + 1 where ordinal >= {position}");
            DbContext.Database.ExecuteSqlCommand($"Update accounts.transactions set ordinal = {position} where id = {id}");
        }

        public void MoveTransactionUp(int accountId, int transactionId)
        {
            var transaction = DbContext.Transactions.Find(transactionId);
            var targetTransaction = GetNextTransactionOnSameDay(accountId, transaction);
            MoveOrdinalFromAtoB(transactionId, transaction.Ordinal, targetTransaction?.Ordinal ?? transaction.Ordinal + 1);
            EventBus.Instance.Publish(new TransactionMoved(accountId, transaction.Date));
        }

        public void MoveTransactionDown(int accountId, int transactionId)
        {
            var transaction = DbContext.Transactions.Find(transactionId);
            var targetTransaction = GetPreviousTransactionOnSameDay(accountId, transaction);
            MoveOrdinalFromAtoB(transactionId, transaction.Ordinal, targetTransaction?.Ordinal ?? transaction.Ordinal + 1);
            EventBus.Instance.Publish(new TransactionMoved(accountId, transaction.Date));
        }

        private Transaction GetNextTransactionOnSameDay(int accountId, Transaction transaction)
        {
            var targetTransaction = DbContext.Transactions
                .Where(x => x.AccountId == accountId && x.Date == transaction.Date && x.Ordinal > transaction.Ordinal)
                .OrderBy(x => x.Ordinal)
                .FirstOrDefault();
            return targetTransaction;
        }

        private Transaction GetPreviousTransactionOnSameDay(int accountId, Transaction transaction)
        {
            var targetTransaction = DbContext.Transactions
                .Where(x => x.AccountId == accountId && x.Date == transaction.Date && x.Ordinal < transaction.Ordinal)
                .OrderByDescending(x => x.Ordinal).FirstOrDefault();
            return targetTransaction;
        }

        private void MoveOrdinalFromAtoB(int id, int start, int end)
        {
            var update = start > end
                ? $"Update accounts.transactions set ordinal = ordinal + 1 where ordinal < {start} and ordinal >= {end}"
                : $"Update accounts.transactions set ordinal = ordinal - 1 where ordinal > {start} and ordinal <= {end}";
            DbContext.Database.ExecuteSqlCommand(update);
            DbContext.Database.ExecuteSqlCommand($"Update accounts.transactions set ordinal = {end} where id = {id}");
        }

        private static IEnumerable<AccountTransactionModel> GetAccountLines(int accountId, IEnumerable<TransactionModel> transactions)
        {
            return transactions.Select(transaction => new AccountTransactionModel
            {
                Id = transaction.Id,
                Description = transaction.Description,
                AccountId = accountId,
                LinkedAccountId = transaction.FromId == accountId ? transaction.ToId : transaction.FromId,
                CategoryId = transaction.CategoryId,
                CategoryName = transaction.CategoryName,
                Amount = transaction.FromId == accountId ? -transaction.Amount : transaction.Amount,
                Balance = transaction.Balance,
                Date = transaction.Date,
            }).ToList();
        }
    }
}