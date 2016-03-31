using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Picassi.Core.Accounts.ViewModels.Transactions;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.Services.Transactions
{
    public interface ITransactionUploadService
    {
        void AddTransactionsToAccount(int accountId, IEnumerable<TransactionUploadModel> transactions);
        void ConfirmTransactions(int accountId, IEnumerable<int> transactionids);
    }

    public class TransactionUploadService : ITransactionUploadService
    {
        private readonly IAccountsDataContext _dbContext;

        public TransactionUploadService(IAccountsDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddTransactionsToAccount(int accountId, IEnumerable<TransactionUploadModel> transactions)
        {
            _dbContext.Transactions.AddRange(transactions.Select(x => CreateTransaction(accountId, x)).Where(x => x != null));
            _dbContext.SaveChanges();
        }

        public void ConfirmTransactions(int accountId, IEnumerable<int> transactionids)
        {
            foreach (var transaction in _dbContext.Transactions.Where(x => transactionids.Contains(x.Id)))
            {
                transaction.Status = TransactionStatus.Confirmed;
            }
            _dbContext.SaveChanges();
        }

        private Transaction CreateTransaction(int baseAccountId, TransactionUploadModel transaction)
        {
            return new Transaction
            {
                Amount = GetTransactionAmount(transaction),
                Date = DateTime.ParseExact(transaction.Date, "dd/MM/yyyy", CultureInfo.CurrentCulture),
                Description = transaction.Description,
                CategoryId = _dbContext.Categories.SingleOrDefault(x => x.Name == transaction.CategoryName)?.Id,
                FromId = (transaction.Amount < 0 || transaction.Amount == 0 && transaction.Debit > 0) ? baseAccountId : (int?)null,
                ToId = (transaction.Amount > 0 || transaction.Amount == 0 && transaction.Credit > 0) ? baseAccountId : (int?)null,
                Status = TransactionStatus.Provisional
            };
        }

        private static decimal GetTransactionAmount(TransactionUploadModel transaction)
        {
            return transaction.Amount == 0
                ? transaction.Credit + transaction.Debit
                : (transaction.Amount > 0 ? transaction.Amount : -transaction.Amount);
        }
    }
}