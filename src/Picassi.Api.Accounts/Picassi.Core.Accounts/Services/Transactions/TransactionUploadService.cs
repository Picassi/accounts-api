using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Picassi.Api.Accounts.Contract.Transactions;
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
        private readonly ICategoryAssignmentService _categoryAssignmentService;
        private readonly ITransactionModelMapper _transactionModelMapper;

        public TransactionUploadService(IAccountsDatabaseProvider dbProvider, ICategoryAssignmentService categoryAssignmentService, ITransactionModelMapper transactionModelMapper)
        {
            _dbProvider = dbProvider;
            _categoryAssignmentService = categoryAssignmentService;
            _transactionModelMapper = transactionModelMapper;
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
                    _dbProvider.GetDataContext().Transactions.Add(_transactionModelMapper.CreateEntity(t));
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
                var date = DateTime.ParseExact(upload.Date, "dd/MM/yyyy", CultureInfo.CurrentCulture);

                var transaction = new TransactionModel
                {
                    Amount = GetTransactionAmount(upload),
                    Date = date,
                    Description = upload.Description,
                    AccountId = baseAccountId,
                    ToId = null,
                    Ordinal = ordinal,
                    Balance = upload.Balance ?? 0
                };

                _categoryAssignmentService.AssignCategory(transaction);

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


        private List<TransactionModel> AmendTransactionOrder(List<TransactionModel> transactions, int ordinal)
        {
            foreach (var candidate in GetCandidateStartTransactions(transactions))
            {
                var results = new List<TransactionModel>();
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

        private IEnumerable<TransactionModel> GetCandidateStartTransactions(List<TransactionModel> transactions)
        {
            var firstDay = transactions.Min(x => x.Date);
            var lastDay = transactions.Max(x => x.Date);
            var total = transactions.Sum(x => x.Amount);
            var candidates = transactions.Where(x => x.Date == firstDay);
            var endpoints = transactions.Where(x => x.Date == lastDay);
            var matchingCandidates = candidates.Where(x => endpoints.Select(e => e.Balance).Contains(x.Balance - x.Amount + total)).ToList();
            return matchingCandidates;
        }

        private static bool TryOrderTransactionsWithStartCandidate(TransactionModel startPoint, IEnumerable<TransactionModel> transactions, ICollection<TransactionModel> results)
        {
            var transactionPool = new List<TransactionModel>();
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
        public TransactionModel Transaction { get; set; }
        public TransactionUploadModel Upload { get; set; }

        public TransactionUploadResult(TransactionUploadModel upload, TransactionModel transaction)
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