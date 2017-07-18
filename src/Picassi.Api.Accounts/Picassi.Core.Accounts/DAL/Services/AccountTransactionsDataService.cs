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
            IAccountsDatabaseProvider dbProvider, ITransactionsDataService transactionsDataService) 
            : base(modelMapper, dbProvider)
        {
            _transactionsDataService = transactionsDataService;
        }

        public override AccountTransactionModel Update(int id, AccountTransactionModel model)
        {
            var modelToReturn = base.Update(id, model);
            EventBus.Instance.Publish(new TransactionMoved(modelToReturn.AccountId, modelToReturn.Date));
            return modelToReturn;
        }

        public ResultsViewModel<AccountTransactionModel> Query(int accountId, AccountTransactionsQueryModel query)
        {
            var transactions = _transactionsDataService.QueryWithCount(query.Text, new[] { accountId }, query.Categories, 
                query.DateFrom, query.DateTo, query.ShowUncategorised, query.PageSize, 
                query.PageNumber, query.SortBy, query.SortAscending);

            return new ResultsViewModel<AccountTransactionModel>
            {
                TotalLines = transactions.TotalLines,
                Lines = GetAccountLines(accountId, transactions.Lines)
            };
        }

        public void InsertTransactionAt(int id, int position)
        {
            DbProvider.GetDataContext().Database.ExecuteSqlCommand($"Update accounts.transactions set ordinal = ordinal + 1 where ordinal >= {position}");
            DbProvider.GetDataContext().Database.ExecuteSqlCommand($"Update accounts.transactions set ordinal = {position} where id = {id}");
        }

        public void MoveTransactionUp(int accountId, int transactionId)
        {
            var transaction = DbProvider.GetDataContext().Transactions.Find(transactionId);
            var targetTransaction = GetNextTransactionOnSameDay(accountId, transaction);
            MoveOrdinalFromAtoB(transactionId, transaction.Ordinal, targetTransaction?.Ordinal ?? transaction.Ordinal + 1);
            EventBus.Instance.Publish(new TransactionMoved(accountId, transaction.Date));
        }

        public void MoveTransactionDown(int accountId, int transactionId)
        {
            var transaction = DbProvider.GetDataContext().Transactions.Find(transactionId);
            var targetTransaction = GetPreviousTransactionOnSameDay(accountId, transaction);
            MoveOrdinalFromAtoB(transactionId, transaction.Ordinal, targetTransaction?.Ordinal ?? transaction.Ordinal + 1);
            EventBus.Instance.Publish(new TransactionMoved(accountId, transaction.Date));
        }

        private Transaction GetNextTransactionOnSameDay(int accountId, Transaction transaction)
        {
            var targetTransaction = DbProvider.GetDataContext().Transactions
                .Where(x => x.AccountId == accountId && x.Date == transaction.Date && x.Ordinal > transaction.Ordinal)
                .OrderBy(x => x.Ordinal)
                .FirstOrDefault();
            return targetTransaction;
        }

        private Transaction GetPreviousTransactionOnSameDay(int accountId, Transaction transaction)
        {
            var targetTransaction = DbProvider.GetDataContext().Transactions
                .Where(x => x.AccountId == accountId && x.Date == transaction.Date && x.Ordinal < transaction.Ordinal)
                .OrderByDescending(x => x.Ordinal).FirstOrDefault();
            return targetTransaction;
        }

        private void MoveOrdinalFromAtoB(int id, int start, int end)
        {
            var update = start > end
                ? $"Update accounts.transactions set ordinal = ordinal + 1 where ordinal < {start} and ordinal >= {end}"
                : $"Update accounts.transactions set ordinal = ordinal - 1 where ordinal > {start} and ordinal <= {end}";
            DbProvider.GetDataContext().Database.ExecuteSqlCommand(update);
            DbProvider.GetDataContext().Database.ExecuteSqlCommand($"Update accounts.transactions set ordinal = {end} where id = {id}");
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
