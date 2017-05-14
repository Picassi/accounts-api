using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Core.Accounts.DbAccess;
using Picassi.Core.Accounts.ViewModels.Transactions;
using Picassi.Utils.Api.Attributes;

namespace Picassi.Api.Accounts.Controllers
{
	[EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class TransactionsController : ApiController
	{
	    private readonly ITransactionsDataService _dataService;

	    public TransactionsController(ITransactionsDataService dataService)
	    {
	        _dataService = dataService;
	    }

	    [HttpGet]
        [Route("transactions")]
        public TransactionsResultsViewModel GetTransactions([FromUri]TransactionsQueryModel query)
        {
            return _dataService.Query(query);
        }

        [HttpPost]
        [Route("transactions")]
        public TransactionModel CreateTransaction([FromBody]TransactionModel transactionModel)
        {
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
        public TransactionModel UpdateTransaction(int id, [FromBody]TransactionModel transactionModel)
        {
            return _dataService.Update(id, transactionModel);
        }

        [HttpDelete]
        [Route("transactions/{id}")]
        public bool DeleteAccount(int id)
        {
            return _dataService.Delete(id);
        }
    }
}