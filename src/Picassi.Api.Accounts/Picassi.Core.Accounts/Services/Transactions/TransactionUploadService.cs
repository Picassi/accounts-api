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
        IList<TransactionUploadResult> AddTransactionsToAccount(int accountId, ICollection<TransactionUploadModel> transactions);
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

        public IList<TransactionUploadResult> AddTransactionsToAccount(int accountId, ICollection<TransactionUploadModel> transactions)
        {
            var maxOrdinal = _dbProvider.GetDataContext().Transactions.Any()
                ? _dbProvider.GetDataContext().Transactions.Max(transaction => transaction.Ordinal)
                : 0;

            var transactionModels = transactions
                .OrderBy(x => x.Ordinal)
                .Select(transaction => CreateTransaction(accountId, transaction, ++maxOrdinal))
                .ToList();
            if (transactionModels.All(t => t.Success))
            {
                var successfulTransactions = transactionModels.Where(t => t.Success).Select(t => t.Transaction).ToList();
                foreach (var t in successfulTransactions)
                {
                    _dbProvider.GetDataContext().Transactions.Add(t);
                }
                _dbProvider.GetDataContext().SaveChanges();
                _balanceService.SetTransactionBalances(accountId, successfulTransactions.Min(transaction => transaction.Date));
            }
            return transactionModels;
        }

        private TransactionUploadResult CreateTransaction(int baseAccountId, TransactionUploadModel upload, int ordinal)
        {

            try
            {
                var transaction = new Transaction
                {
                    Amount = GetTransactionAmount(upload),
                    Date = DateTime.ParseExact(upload.Date, "dd/MM/yyyy", CultureInfo.CurrentCulture),
                    Description = upload.Description,
                    CategoryId = _dbProvider.GetDataContext().Categories.SingleOrDefault(x => x.Name == upload.CategoryName)?.Id,
                    AccountId = baseAccountId,
                    ToId = null,
                    Ordinal = ordinal
                };
                return new TransactionUploadResult(upload, transaction);
            }
            catch (Exception e)
            {
                return new TransactionUploadResult(upload, e);
            }
        }

        private static decimal GetTransactionAmount(TransactionUploadModel transaction)
        {
            return transaction.Amount == 0 ? transaction.Credit - transaction.Debit : transaction.Amount;
        }
    }

    public class TransactionUploadResult
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public Transaction Transaction { get; set; }
        public TransactionUploadModel Upload { get; set; }

        public TransactionUploadResult(TransactionUploadModel upload, Transaction transaction)
        {
            Upload = upload;
            Transaction = transaction;
            Success = true;
        }

        public TransactionUploadResult(TransactionUploadModel upload, Exception exc)
        {
            Upload = upload;
            Error = exc.Message;
            Success = false;
        }
    }
}