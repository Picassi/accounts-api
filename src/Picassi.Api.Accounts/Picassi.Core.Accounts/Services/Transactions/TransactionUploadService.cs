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
        private readonly IAccountsDatabaseProvider _dbProvider;
        private readonly IAccountBalanceService _balanceService;

        public TransactionUploadService(IAccountsDatabaseProvider dbProvider, IAccountBalanceService balanceService)
        {
            _dbProvider = dbProvider;
            _balanceService = balanceService;
        }

        public void AddTransactionsToAccount(int accountId, ICollection<TransactionUploadModel> transactions)
        {
            var maxOrdinal = _dbProvider.GetDataContext().Transactions.Any()
                ? _dbProvider.GetDataContext().Transactions.Max(transaction => transaction.Ordinal)
                : 0;

            var transactionModels = transactions
                .OrderBy(x => x.Ordinal)
                .Select(transaction => CreateTransaction(accountId, transaction, ++maxOrdinal))
                .ToList();

            _dbProvider.GetDataContext().Transactions.AddRange(transactionModels);
            _dbProvider.GetDataContext().SaveChanges();
            _balanceService.SetTransactionBalances(accountId, transactionModels.Min(transaction => transaction.Date));
        }

        private Transaction CreateTransaction(int baseAccountId, TransactionUploadModel transaction, int ordinal)
        {
            return new Transaction
            {
                Amount = GetTransactionAmount(transaction),
                Date = DateTime.ParseExact(transaction.Date, "dd/MM/yyyy", CultureInfo.CurrentCulture),
                Description = transaction.Description,
                CategoryId = _dbProvider.GetDataContext().Categories.SingleOrDefault(x => x.Name == transaction.CategoryName)?.Id,
                AccountId = baseAccountId,
                ToId = null,
                Ordinal = ordinal
            };
        }

        private static decimal GetTransactionAmount(TransactionUploadModel transaction)
        {
            return transaction.Amount == 0 ? transaction.Credit - transaction.Debit : transaction.Amount;
        }
    }
}