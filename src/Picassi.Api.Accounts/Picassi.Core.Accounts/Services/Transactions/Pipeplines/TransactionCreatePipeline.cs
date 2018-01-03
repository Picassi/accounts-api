using System.Threading.Tasks;
using Picassi.Api.Accounts.Contract.Transactions;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Services.AssignmentRules;

namespace Picassi.Core.Accounts.Services.Transactions.Pipeplines
{
    public interface ITransactionCreatePipeline
    {
        Task<TransactionModel> CreateTransaction(TransactionModel transactionModel, TransactionUpdateOptions options);
    }

    public class TransactionCreatePipeline : ITransactionCreatePipeline
    {
        private readonly ITransactionsDataService _transactionsDataService;
        private readonly IAccounStatementIntegrityService _accounStatementIntegrityService;
        private readonly IAssignmentRuleGenerator _assignmentRuleGenerator;

        public TransactionCreatePipeline(ITransactionsDataService transactionsDataService, IAccounStatementIntegrityService accounStatementIntegrityService, IAssignmentRuleGenerator assignmentRuleGenerator)
        {
            _transactionsDataService = transactionsDataService;
            _accounStatementIntegrityService = accounStatementIntegrityService;
            _assignmentRuleGenerator = assignmentRuleGenerator;
        }

        public async Task<TransactionModel> CreateTransaction(TransactionModel transactionModel, TransactionUpdateOptions options)
        {
            var updatedTransaction = _transactionsDataService.Create(transactionModel);

            if (options?.AutoGenerateRules == true)
            {
                _assignmentRuleGenerator.GenerateRule(transactionModel, options);
            }

            await _accounStatementIntegrityService.OnTransactionsCreated(updatedTransaction.AccountId, updatedTransaction);

            return updatedTransaction;
        }
    }
}
