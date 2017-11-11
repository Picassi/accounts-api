using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Api.Accounts.Contract;
using Picassi.Api.Accounts.Contract.Transactions;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.Transactions;
using Picassi.Core.Accounts.Services.Transactions;
using Picassi.Core.Accounts.Services.Transactions.Pipeplines;

namespace Picassi.Api.Accounts.Controllers
{
	[EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class TransactionsController : ApiController
	{
	    private readonly ITransactionsDataService _dataService;
	    private readonly ITransactionCreatePipeline _transactionCreatePipeline;
	    private readonly ITransactionUpdatePipeline _transactionUpdatePipeline;
	    private readonly ITransactionUploadPipeline _transactionUploadPipeline;

	    public TransactionsController(ITransactionsDataService dataService, ITransactionCreatePipeline transactionCreatePipeline, ITransactionUpdatePipeline transactionUpdatePipeline, ITransactionUploadPipeline transactionUploadPipeline)
	    {
	        _dataService = dataService;
	        _transactionCreatePipeline = transactionCreatePipeline;
	        _transactionUpdatePipeline = transactionUpdatePipeline;
	        _transactionUploadPipeline = transactionUploadPipeline;
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
        public async Task<TransactionModel> CreateTransaction([FromBody]TransactionModel transactionModel, [FromUri]TransactionUpdateOptions options)
        {
            return await _transactionCreatePipeline.CreateTransaction(transactionModel, options);
        }

        [HttpGet]
        [Route("transactions/{id}")]
        public TransactionModel GetTransaction(int id)
        {
            return _dataService.Get(id);            
        }

        [HttpPut]
        [Route("transactions/{id}")]
        public async Task<TransactionModel> UpdateTransaction(int id, [FromBody]TransactionModel transactionModel, [FromUri]TransactionUpdateOptions options)
        {
            return await _transactionUpdatePipeline.UpdateTransaction(id, transactionModel, options);
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

	    [HttpPost]
	    [Route("accounts/{accountId}/transactions/upload")]
	    public async Task<IList<TransactionUploadRecord>> UploadProcessedTransactions(int accountId, [FromBody]TransactionUploadModel[] transactions)
	    {
	        return await _transactionUploadPipeline.UploadTransactions(accountId, transactions.ToList());
	    }

	    [HttpPost]
	    [Route("accounts/{accountId}/transactions/moveup")]
	    public void MoveTransactionUp(int accountId, [FromBody]int transactionId)
	    {
	        _dataService.MoveTransactionUp(accountId, transactionId);
	    }

	    [HttpPost]
	    [Route("accounts/{accountId}/transactions/movedown")]
	    public void MoveTransactionDown(int accountId, [FromBody]int transactionId)
	    {
	        _dataService.MoveTransactionDown(accountId, transactionId);
	    }

    }
}