using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.ScheduledTransactions;
using Picassi.Utils.Api.Attributes;

namespace Picassi.Api.Accounts.Controllers
{
	[EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class ScheduledTransactionsController : ApiController
	{
	    private readonly IScheduledTransactionsDataService _dataService;

	    public ScheduledTransactionsController(IScheduledTransactionsDataService dataService)
	    {
	        _dataService = dataService;
	    }

	    [HttpGet]
        [Route("scheduled-transactions")]
        public IEnumerable<ScheduledTransactionModel> GetTransactions([FromUri]ScheduledTransactionQueryModel query)
        {
            return _dataService.Query(query);
        }

        [HttpPost]
        [Route("scheduled-transactions")]
        public ScheduledTransactionModel  CreateTransaction([FromBody]ScheduledTransactionModel  transactionModel)
        {
            return _dataService.Create(transactionModel);
        }

        [HttpGet]
        [Route("scheduled-transactions/{id}")]
        public ScheduledTransactionModel  GetTransaction(int id)
        {
            return _dataService.Get(id);            
        }

        [HttpPut]
        [Route("scheduled-transactions/{id}")]
        public ScheduledTransactionModel  UpdateTransaction(int id, [FromBody]ScheduledTransactionModel  transactionModel)
        {
            return _dataService.Update(id, transactionModel);
        }

        [HttpDelete]
        [Route("scheduled-transactions/{id}")]
        public bool DeleteAccount(int id)
        {
            return _dataService.Delete(id);
        }
    }
}