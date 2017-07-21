using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models;
using Picassi.Core.Accounts.Models.Transactions;
using Picassi.Core.Accounts.Services.Transactions;
using Picassi.Utils.Api.Attributes;

namespace Picassi.Api.Accounts.Controllers
{
	[EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class AccountTransactionsController : ApiController
	{
        private readonly IAccountTransactionsDataService _transactionDataService;
        private readonly ITransactionUploadService _transactionUploadService;

	    public AccountTransactionsController(IAccountTransactionsDataService transactionDataService, ITransactionUploadService transactionUploadService)
	    {
	        _transactionDataService = transactionDataService;
	        _transactionUploadService = transactionUploadService;
	    }

	    [HttpGet]
        [Route("accounts/{accountId}/transactions")]
        public ResultsViewModel<AccountTransactionModel> GetTransactionsForAccount(int accountId, [FromUri]AccountTransactionsQueryModel query)
        {
            return _transactionDataService.Query(accountId, query);
        }

        [HttpPost]
        [Route("accounts/{accountId}/transactions")]
        public AccountTransactionModel CreateTransaction(int accountId, [FromBody]AccountTransactionModel transactionModel)
        {
            return _transactionDataService.Create(transactionModel);
        }

        [HttpGet]
        [Route("accounts/{accountId}/transactions/{transactionId}")]
        public AccountTransactionModel GetTransaction(int accountId, int transactionId)
        {
            return _transactionDataService.Get(transactionId);
        }

        [HttpPut]
        [Route("accounts/{accountId}/transactions/{transactionId}")]
        public AccountTransactionModel UpdateTransaction(int accountId, int transactionId, [FromBody]AccountTransactionModel transactionModel)
        {
            return _transactionDataService.Update(transactionId, transactionModel);
        }

        [HttpDelete]
        [Route("accounts/{accountId}/transactions/{transactionId}")]
        public bool DeleteAccount(int accountId, int transactionId)
        {
            return _transactionDataService.Delete(transactionId);
        }


        [HttpPost]
        [Route("accounts/{accountId}/transactions/upload")]
        public IList<TransactionUploadResult> UploadProcessedTransactions(int accountId, [FromBody]TransactionUploadModel[] transactions)
        {
           return _transactionUploadService.AddTransactionsToAccount(accountId, transactions.ToList());
        }

        [HttpPost]
        [Route("accounts/{accountId}/transactions/moveup")]
        public void MoveTransactionUp(int accountId, [FromBody]int transactionId)
        {
            _transactionDataService.MoveTransactionUp(accountId, transactionId);
        }

        [HttpPost]
        [Route("accounts/{accountId}/transactions/movedown")]
        public void MoveTransactionDown(int accountId, [FromBody]int transactionId)
        {
            _transactionDataService.MoveTransactionDown(accountId, transactionId);
        }
    }
}