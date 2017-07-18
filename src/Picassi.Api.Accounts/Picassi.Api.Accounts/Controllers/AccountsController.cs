using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.Accounts;
using Picassi.Core.Accounts.Services.Reports;
using Picassi.Core.Accounts.Time;
using Picassi.Utils.Api.Attributes;

namespace Picassi.Api.Accounts.Controllers
{
	[EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class AccountsController : ApiController
	{
        private readonly IAccountDataService _dataService;
	    private readonly IAccountSummariser _accountSummariser;

        public AccountsController(
            IAccountDataService dataService, 
            IAccountSummariser accountSummariser)
        {
            _dataService = dataService;
            _accountSummariser = accountSummariser;
        }

        [HttpGet]
        [Route("accounts")]
        public IEnumerable<AccountModel> GetAccounts([FromUri]AccountQueryModel query)
        {
            return _dataService.Query(query);
        }

        [HttpPost]
        [Route("accounts")]
        public AccountModel CreateAccount([FromBody]AccountModel accountModel)
        {
            return _dataService.Create(accountModel);
        }

        [HttpGet]
        [Route("accounts/{id}")]
        public AccountModel GetAccount(int id)
        {
            return _dataService.Get(id);            
        }

	    [HttpPut]
        [Route("accounts/{id}")]
        public AccountModel UpdateAccount(int id, [FromBody]AccountModel accountModel)
        {
            return _dataService.Update(id, accountModel);
        }

        [HttpDelete]
        [Route("accounts/{id}")]
        public bool DeleteAccount(int id)
        {
            return _dataService.Delete(id);
        }

        [HttpGet]
        [Route("accounts/{id}/summary")]
        public AccountSummaryViewModel GetAccountSummary(int id, [FromUri]DateRange period)
        {
            return _accountSummariser.GetAccountSummary(id, period.Start, period.End);
        }

        [HttpGet]
        [Route("accounts/summary")]
        public AccountSummaryViewModel GetAccountsSummary([FromUri]DateRange period)
        {
            return _accountSummariser.GetAccountSummary(null, period.Start, period.End);
        }
    }
}