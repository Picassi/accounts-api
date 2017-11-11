using System.Collections.Generic;
using System.Linq;
using Picassi.Api.Accounts.Contract.Transactions;

namespace Picassi.Core.Accounts.Services.Transactions
{
    public interface IUploadSorter
    {
        void SetTransactionOrdinals(IList<TransactionUploadRecord> transactions, CreateTransactionContext context);
    }

    public class UploadSorter : IUploadSorter
    {
        public void SetTransactionOrdinals(IList<TransactionUploadRecord> transactions, CreateTransactionContext context)
        {
            var ordinal = 1;
            var requests = transactions.Select(t => t.Request).ToList();
            foreach (var candidate in GetCandidateStartTransactions(requests))
            {
                var results = new List<CreateTransactionRequest>();
                if (!TryOrderTransactionsWithStartCandidate(candidate, requests, results)) continue;

                foreach (var transaction in results)
                {
                    transaction.Ordinal = ordinal;
                    ordinal++;
                }
                return;
            }

            foreach (var transaction in requests)
            {
                transaction.Ordinal = ordinal;
                ordinal++;
            }
        }

        private IEnumerable<CreateTransactionRequest> GetCandidateStartTransactions(List<CreateTransactionRequest> transactions)
        {
            var firstDay = transactions.Min(x => x.Date);
            var lastDay = transactions.Max(x => x.Date);
            var total = transactions.Sum(x => x.Amount);
            var candidates = transactions.Where(x => x.Date == firstDay);
            var endpoints = transactions.Where(x => x.Date == lastDay);
            var matchingCandidates = candidates
                .Where(x => x.Balance != null && endpoints.Where(e => e.Balance != null)
                    .Select(e => (decimal)e.Balance)
                    .Contains((decimal)x.Balance - x.Amount + total)).ToList();
            return matchingCandidates;
        }

        private static bool TryOrderTransactionsWithStartCandidate(CreateTransactionRequest startPoint, 
            IEnumerable<CreateTransactionRequest> transactions, ICollection<CreateTransactionRequest> results)
        {
            var transactionPool = new List<CreateTransactionRequest>();
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

}
