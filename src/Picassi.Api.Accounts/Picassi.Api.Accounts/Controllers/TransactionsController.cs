using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Common.Api.Attributes;
using Picassi.Core.Accounts.DbAccess.Transactions;
using Picassi.Core.Accounts.ViewModels.Transactions;

namespace Picassi.Api.Accounts.Controllers
{
	[EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class TransactionsController : ApiController
    {
        private readonly ITransactionCrudService _crudService;
        private readonly ITransactionQueryService _queryService;

	    public TransactionsController(
            ITransactionCrudService crudService, 
            ITransactionQueryService queryService)
        {
            _crudService = crudService;
            _queryService = queryService;
        }

        [HttpGet]
        [Route("transactions")]
        public IEnumerable<TransactionViewModel> GetTransactions([FromUri]TransactionsQueryModel query)
        {
            return _queryService.Query(query);
        }

        [HttpPost]
        [Route("transactions")]
        public TransactionViewModel CreateTransaction([FromBody]TransactionViewModel transactionViewModel)
        {
            return _crudService.CreateTransaction(transactionViewModel);
        }

        [HttpGet]
        [Route("transactions/{id}")]
        public TransactionViewModel GetTransaction(int id)
        {
            return _crudService.GetTransaction(id);            
        }

        [HttpPut]
        [Route("transactions/{id}")]
        public TransactionViewModel UpdateTransaction(int id, [FromBody]TransactionViewModel transactionViewModel)
        {
            return _crudService.UpdateTransaction(id, transactionViewModel);
        }

        [HttpDelete]
        [Route("transactions/{id}")]
        public bool DeleteAccount(int id)
        {
            return _crudService.DeleteTransaction(id);
        }
    }
}