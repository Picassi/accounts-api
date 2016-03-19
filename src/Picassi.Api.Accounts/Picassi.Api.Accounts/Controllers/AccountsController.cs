using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Common.Api.Attributes;
using Picassi.Core.Accounts.DbAccess.Accounts;
using Picassi.Core.Accounts.Reports;
using Picassi.Core.Accounts.ViewModels.Accounts;

namespace Picassi.Api.Accounts.Controllers
{
	[EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class AccountsController : ApiController
	{
        private readonly IAccountCrudService _crudService;
        private readonly IAccountQueryService _queryService;
	    private readonly IAccountSummariser _accountSummariser;

        public AccountsController(
            IAccountCrudService crudService, 
            IAccountQueryService queryService, 
            IAccountSummariser accountSummariser)
        {
            _crudService = crudService;
            _queryService = queryService;
            _accountSummariser = accountSummariser;
        }

        [HttpGet]
        [Route("accounts")]
        public IEnumerable<AccountViewModel> GetAccounts([FromUri]AccountQueryModel query)
        {
            return _queryService.Query(query);
        }

        [HttpPost]
        [Route("accounts")]
        public AccountViewModel CreateAccount([FromBody]AccountViewModel accountViewModel)
        {
            return _crudService.CreateAccount(accountViewModel);
        }

        [HttpGet]
        [Route("accounts/{id}")]
        public AccountViewModel GetAccount(int id)
        {
            return _crudService.GetAccount(id);            
        }

        [HttpGet]
        [Route("accounts/{id}/summary")]
        public AccountSummaryViewModel GetAccountSummary([FromUri]AccountPeriodViewModel period)
        {
            return _accountSummariser.GetAccountSummary(period);
        }

	    [HttpPut]
        [Route("accounts/{id}")]
        public AccountViewModel UpdateAccount(int id, [FromBody]AccountViewModel accountViewModel)
        {
            return _crudService.UpdateAccount(id, accountViewModel);
        }

        [HttpDelete]
        [Route("accounts/{id}")]
        public bool DeleteAccount(int id)
        {
            return _crudService.DeleteAccount(id);
        }
	}
}