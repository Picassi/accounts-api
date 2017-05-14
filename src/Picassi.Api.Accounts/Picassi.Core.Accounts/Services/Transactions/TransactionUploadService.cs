using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Picassi.Core.Accounts.DAL;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Models.Transactions;
using Picassi.Core.Accounts.Services.Reports;

namespace Picassi.Core.Accounts.Services.Transactions
{
    public interface ITransactionUploadService
    {
        void AddTransactionsToAccount(int accountId, ICollection<TransactionUploadModel> transactions);
    }

    public class TransactionUploadService : ITransactionUploadService
    {
        private readonly IAccountsDataContext _dbContext;
        private readonly IAccountBalanceService _balanceService;

        public TransactionUploadService(IAccountsDataContext dbContext, IAccountBalanceService balanceService)
        {
            _dbContext = dbContext;
            _balanceService = balanceService;
        }

        public void AddTransactionsToAccount(int accountId, ICollection<TransactionUploadModel> transactions)
        {
            var maxOrdinal = _dbContext.Transactions.Any()
                ? _dbContext.Transactions.Max(transaction => transaction.Ordinal)
                : 0;

            var transactionModels = transactions
                .OrderBy(x => x.Ordinal)
                .Select(transaction => CreateTransaction(accountId, transaction, ++maxOrdinal))
                .ToList();

            _dbContext.Transactions.AddRange(transactionModels);
            _dbContext.SaveChanges();
            _balanceService.SetTransactionBalances(accountId, transactionModels.Min(transaction => transaction.Date));
        }

        private Transaction CreateTransaction(int baseAccountId, TransactionUploadModel transaction, int ordinal)
        {
            return new Transaction
            {
                Amount = GetTransactionAmount(transaction),
                Date = DateTime.ParseExact(transaction.Date, "dd/MM/yyyy", CultureInfo.CurrentCulture),
                Description = transaction.Description,
                CategoryId = _dbContext.Categories.SingleOrDefault(x => x.Name == transaction.CategoryName)?.Id,
                FromId = (transaction.Amount < 0 || transaction.Amount == 0 && transaction.Debit > 0) ? baseAccountId : (int?)null,
                ToId = (transaction.Amount > 0 || transaction.Amount == 0 && transaction.Credit > 0) ? baseAccountId : (int?)null,
                Ordinal = ordinal
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