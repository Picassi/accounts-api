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
                var successfulTransactions = transactionModels.Select(t => t.Transaction).ToList();

                var orderedTransactions = AmendTransactionOrder(successfulTransactions, maxOrdinal + 1);

                foreach (var t in orderedTransactions)
                {
                    _dbProvider.GetDataContext().Transactions.Add(t);
                }
                _dbProvider.GetDataContext().SaveChanges();
                //_balanceService.SetTransactionBalances(accountId, orderedTransactions.First().Balance = orderedTransactions.First);
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
                    Ordinal = ordinal,
                    Balance = upload.Balance ?? 0
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
            return transaction.Amount ?? (transaction.Credit ?? 0) - (transaction.Debit ?? 0);
        }


        private List<Transaction> AmendTransactionOrder(List<Transaction> transactions, int ordinal)
        {
            foreach (var candidate in GetCandidateStartTransactions(transactions))
            {
                var results = new List<Transaction>();
                if (TryOrderTransactionsWithStartCandidate(candidate, transactions, results))
                {
                    foreach (var transaction in results)
                    {
                        transaction.Ordinal = ordinal;
                        ordinal++;
                    }
                    return results;
                }
            }
            return transactions;
        }

        private IEnumerable<Transaction> GetCandidateStartTransactions(List<Transaction> transactions)
        {
            var firstDay = transactions.Min(x => x.Date);
            var lastDay = transactions.Max(x => x.Date);
            var total = transactions.Sum(x => x.Amount);
            var candidates = transactions.Where(x => x.Date == firstDay);
            var endpoints = transactions.Where(x => x.Date == lastDay);
            var matchingCandidates = candidates.Where(x => endpoints.Select(e => e.Balance).Contains(x.Balance - x.Amount + total)).ToList();
            return matchingCandidates;
        }

        private static bool TryOrderTransactionsWithStartCandidate(Transaction startPoint, IEnumerable<Transaction> transactions, ICollection<Transaction> results)
        {
            var transactionPool = new List<Transaction>();
            transactionPool.AddRange(transactions);
            transactionPool.Remove(startPoint);

            while (transactionPool.Count > 0)
            {
                var nextTransaction = transactionPool.FirstOrDefault(x =>
                    x.Date == transactionPool.Min(p => p.Date) && x.Balance == startPoint.Balance + x.Amount);
                if (nextTransaction == null) return false;

                transactionPool.Remove(nextTransaction);
                results.Add(nextTransaction);
                startPoint = nextTransaction;
            }

            return true;
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