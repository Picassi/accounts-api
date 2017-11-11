using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.ModelledTransactions;

namespace Picassi.Core.Accounts.Services.Budgets.Pipelines
{
    public interface IDeleteBudgetPipeline
    {
        bool Delete(int budgetId);
    }

    public class DeleteBudgetPipeline : IDeleteBudgetPipeline
    {
        private readonly IModelledTransactionsDataService _modelledTransactionsDataService;
        private readonly IBudgetsDataService _budgetsDataService;

        public DeleteBudgetPipeline(IBudgetsDataService budgetsDataService, IModelledTransactionsDataService modelledTransactionsDataService)
        {
            _budgetsDataService = budgetsDataService;
            _modelledTransactionsDataService = modelledTransactionsDataService;
        }

        public bool Delete(int budgetId)
        {
            foreach (var modelledTransaction in _modelledTransactionsDataService.Query(budgets: new[] { budgetId }).Lines)
            {
                _modelledTransactionsDataService.Delete(modelledTransaction.Id);
            }

            return _budgetsDataService.Delete(budgetId);
        }
    }
}
