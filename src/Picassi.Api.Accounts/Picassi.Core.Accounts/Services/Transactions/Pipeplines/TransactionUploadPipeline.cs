using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Picassi.Core.Accounts.Models.Transactions;

namespace Picassi.Core.Accounts.Services.Transactions.Pipeplines
{
    public interface ITransactionUploadPipeline
    {
        Task<IList<TransactionUploadRecord>> UploadTransactions(int accountId, ICollection<TransactionUploadModel> transactions);
    }

    public class TransactionUploadPipeline : ITransactionUploadPipeline
    {
        private readonly ICreateTransactionContextBuilder _contextBuilder;
        private readonly IUploadSorter _uploadSorter;
        private readonly ICategoryAssignmentService _categoryAssignmentService;
        private readonly IBalanceCalculator _balanceCalculator;
        private readonly IScheduledTransactionLinks _scheduledTransactionLinks;
        private readonly ICreateTransactionOperation _operation;
        private readonly IAccounStatementIntegrityService _accounStatementIntegrityService;

        public TransactionUploadPipeline(ICreateTransactionContextBuilder contextBuilder, IUploadSorter uploadSorter, ICategoryAssignmentService categoryAssignmentService, IBalanceCalculator balanceCalculator, IScheduledTransactionLinks scheduledTransactionLinks, ICreateTransactionOperation operation, IAccounStatementIntegrityService accounStatementIntegrityService)
        {
            _contextBuilder = contextBuilder;
            _uploadSorter = uploadSorter;
            _categoryAssignmentService = categoryAssignmentService;
            _balanceCalculator = balanceCalculator;
            _scheduledTransactionLinks = scheduledTransactionLinks;
            _operation = operation;
            _accounStatementIntegrityService = accounStatementIntegrityService;
        }

        public async Task<IList<TransactionUploadRecord>> UploadTransactions(int accountId, ICollection<TransactionUploadModel> transactions)
        {
            var uploads = GenerateUploadRecords(accountId, transactions);
            var context = GenerateContext(accountId, uploads);

            _uploadSorter.SetTransactionOrdinals(uploads, context);
            _categoryAssignmentService.AssignCategories(uploads, context);
            //_balanceCalculator.SetBalances(uploads, context);
            //_scheduledTransactionLinks.LinkScheduledTransactions(uploads, context);            

            CreateTransactions(uploads, context);

            var uploadsToProcess = uploads
                .Where(u => u.Transaction != null && u.Request.Balance != null)
                .Select(u => u.Transaction).ToList();
            await _accounStatementIntegrityService.OnTransactionsCreated(accountId, uploadsToProcess);

            return uploads;
        }

        private static IList<TransactionUploadRecord> GenerateUploadRecords(int accountId, IEnumerable<TransactionUploadModel> uploadedTransactions)
        {
            return uploadedTransactions
                .Select(transaction => new TransactionUploadRecord(accountId, transaction))
                .OrderBy(transaction => transaction.Request.Date)
                .ThenBy(transaction => transaction.Upload.Ordinal ?? 0)
                .ToList();
        }

        private CreateTransactionContext GenerateContext(int accountId, IEnumerable<TransactionUploadRecord> requests)
        {
            var firstTransactionDate = requests.Min(r => r.Request.Date);
            return _contextBuilder.BuildContext(accountId, firstTransactionDate);
        }

        private void CreateTransactions(IEnumerable<TransactionUploadRecord> uploads, CreateTransactionContext context)
        {
            foreach (var upload in uploads)
            {
                CreateTransaction(context, upload);
            }
        }

        private void CreateTransaction(CreateTransactionContext context, TransactionUploadRecord upload)
        {
            try
            {
                var model = _operation.CreateTransaction(upload.Request, context);
                upload.RecordSuccess(model);
            }
            catch (Exception exc)
            {
                upload.RecordError(exc.Message);
            }
        }
    }
}
