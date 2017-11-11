using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.Budgets;
using Picassi.Core.Accounts.Services.Budgets;
using Picassi.Core.Accounts.Services.Budgets.Pipelines;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class BudgetsController : ApiController
    {
        private readonly IBudgetsDataService _dataService;
        private readonly IBudgetReportsCompiler _budgetReportsCompiler;
        private readonly IDeleteBudgetPipeline _deleteBudgetPipeline;

        public BudgetsController(IBudgetsDataService dataService, IBudgetReportsCompiler budgetReportsCompiler, IDeleteBudgetPipeline deleteBudgetPipeline)
        {
            _dataService = dataService;
            _budgetReportsCompiler = budgetReportsCompiler;
            _deleteBudgetPipeline = deleteBudgetPipeline;
        }

        [HttpGet]
        [Route("budgets")]
        public IEnumerable<BudgetModel> GetBudgets([FromUri]BudgetsQueryModel query)
        {
            return _dataService.Query(query);
        }

        [HttpPost]
        [Route("budgets")]
        public BudgetModel CreateBudget([FromBody]BudgetModel model)
        {
            return _dataService.Create(model);
        }

        [HttpGet]
        [Route("budgets/{id}")]
        public BudgetModel GetBudget(int id)
        {
            return _dataService.Get(id);
        }

        [HttpPut]
        [Route("budgets/{id}")]
        public BudgetModel UpdateBudget(int id, [FromBody]BudgetModel budgetModel)
        {
            return _dataService.Update(id, budgetModel);
        }

        [HttpDelete]
        [Route("budgets/{id}")]
        public bool DeleteBudget(int id)
        {
            return _deleteBudgetPipeline.Delete(id);
        }

        [HttpPost]
        [Route("budgets/progress")]
        public IEnumerable<BudgetSummary> GetBudgetReports(BudgetsQueryModel query)
        {
            return _budgetReportsCompiler.GetBudgetReports(query);
        }

    }
}