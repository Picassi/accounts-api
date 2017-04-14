using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Core.Accounts.DbAccess.Transactions;
using Picassi.Core.Accounts.Services.Transactions;
using Picassi.Core.Accounts.ViewModels;
using Picassi.Core.Accounts.ViewModels.Transactions;
using Picassi.Utils.Api.Attributes;

namespace Picassi.Api.Accounts.Controllers
{
	[EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class AccountTransactionsController : ApiController
	{
	    private readonly IAccountTransactionCrudService _transactionCrudService;
        private readonly ITransactionQueryService _transactionQueryService;
        private readonly ITransactionUploadService _transactionUploadService;

	    public AccountTransactionsController(IAccountTransactionCrudService transactionTransactionCrudService, ITransactionQueryService transactionQueryService, ITransactionUploadService transactionUploadService)
	    {
	        _transactionCrudService = transactionTransactionCrudService;
	        _transactionQueryService = transactionQueryService;
	        _transactionUploadService = transactionUploadService;
	    }

	    [HttpGet]
        [Route("accounts/{accountId}/transactions")]
        public ResultsViewModel<AccountTransactionViewModel> GetTransactionsForAccount(int accountId, [FromUri]AccountTransactionsQueryModel query)
        {
            return _transactionQueryService.Query(accountId, query);
        }

        [HttpPost]
        [Route("accounts/{accountId}/transactions")]
        public AccountTransactionViewModel CreateTransaction(int accountId, [FromBody]AccountTransactionViewModel transactionViewModel)
        {
            return _transactionCrudService.CreateTransaction(accountId, transactionViewModel);
        }

        [HttpGet]
        [Route("accounts/{accountId}/transactions/{transactionId}")]
        public AccountTransactionViewModel GetTransaction(int accountId, int transactionId)
        {
            return _transactionCrudService.GetTransaction(accountId, transactionId);
        }

        [HttpPut]
        [Route("accounts/{accountId}/transactions/{transactionId}")]
        public AccountTransactionViewModel UpdateTransaction(int accountId, int transactionId, [FromBody]AccountTransactionViewModel transactionViewModel)
        {
            return _transactionCrudService.UpdateTransaction(accountId, transactionId, transactionViewModel);
        }

        [HttpDelete]
        [Route("accounts/{accountId}/transactions/{transactionId}")]
        public bool DeleteAccount(int accountId, int transactionId)
        {
            return _transactionCrudService.DeleteTransaction(accountId, transactionId);
        }


        [HttpPost]
        [Route("accounts/{accountId}/transactions/upload")]
        public void UploadProcessedTransactions(int accountId, [FromBody]TransactionUploadModel[] transactions)
        {
           _transactionUploadService.AddTransactionsToAccount(accountId, transactions.ToList());
        }

        [HttpPost]
        [Route("accounts/{accountId}/transactions/confirm")]
        public void Confirm(int accountId, [FromBody]int[] transactionIds)
        {
            _transactionUploadService.ConfirmTransactions(accountId, transactionIds);
        }

        [HttpPost]
        [Route("accounts/{accountId}/transactions/moveup")]
        public void MoveTransactionUp(int accountId, [FromBody]int transactionId)
        {
            _transactionCrudService.MoveTransactionUp(accountId, transactionId);
        }

        [HttpPost]
        [Route("accounts/{accountId}/transactions/movedown")]
        public void MoveTransactionDown(int accountId, [FromBody]int transactionId)
        {
            _transactionCrudService.MoveTransactionDown(accountId, transactionId);
        }
    }
}