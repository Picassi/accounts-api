using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Api.Accounts.Contract;
using Picassi.Api.Accounts.Contract.Transactions;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Services.AssignmentRules;

namespace Picassi.Api.Accounts.Controllers
{
	[EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class TransactionsController : ApiController
	{
	    private readonly ITransactionsDataService _dataService;
	    private readonly IAssignmentRuleGenerator _assignmentRuleGenerator; 

	    public TransactionsController(ITransactionsDataService dataService, IAssignmentRuleGenerator assignmentRuleGenerator)
	    {
	        _dataService = dataService;
	        _assignmentRuleGenerator = assignmentRuleGenerator;
	    }

	    [HttpGet]
        [Route("transactions")]
        public TransactionsResultsViewModel GetTransactions([FromUri]TransactionsQueryModel query)
        {
            return _dataService.QueryWithCount(query?.Text, query?.Accounts, query?.Categories, query?.DateFrom, query?.DateTo, 
                query?.ShowUncategorised, query?.ShowAllCategorised, query?.ShowSubcategories, query?.PageSize, query?.PageNumber, query?.SortBy, query?.SortAscending);
        }

        [HttpPost]
        [Route("transactions")]
        public TransactionModel CreateTransaction([FromBody]TransactionModel transactionModel, [FromUri]TransactionUpdateOptions options)
        {
            if (options?.AutoGenerateRules == true)
            {
                _assignmentRuleGenerator.GenerateRule(transactionModel, options);
            }
            return _dataService.Create(transactionModel);
        }

        [HttpGet]
        [Route("transactions/{id}")]
        public TransactionModel GetTransaction(int id)
        {
            return _dataService.Get(id);            
        }

        [HttpPut]
        [Route("transactions/{id}")]
        public TransactionModel UpdateTransaction(int id, [FromBody]TransactionModel transactionModel, [FromUri]TransactionUpdateOptions options)
        {
            if (options?.AutoGenerateRules == true)
            {
                _assignmentRuleGenerator.GenerateRule(transactionModel, options);
            }
            return _dataService.Update(id, transactionModel);
        }

        [HttpDelete]
        [Route("transactions/{id}")]
        public bool DeleteAccount(int id)
        {
            return _dataService.Delete(id);
        }

	    [HttpPost]
	    [Route("actions/transactions/bulk/set-recurrence")]
	    public IEnumerable<TransactionModel> SetTransactionRecurrence([FromBody]SetTransactionRecurrenceRequest model)
	    {
	        return _dataService.SetTransactionRecurrences(model.TransactionIds, model.Recurrence);
	    }
    }
}