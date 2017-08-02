using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Api.Accounts.Contract;
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
	    private readonly IAccountBalanceService _accountBalanceService;

        public AccountsController(
            IAccountDataService dataService, 
            IAccountSummariser accountSummariser, IAccountBalanceService accountBalanceService)
        {
            _dataService = dataService;
            _accountSummariser = accountSummariser;
            _accountBalanceService = accountBalanceService;
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
            var start = period?.Start ?? DateTime.Today.AddMonths(-1);
            var end = period?.End ?? DateTime.Today;
            return _accountSummariser.GetAccountSummary(id, start, end);
        }

	    [HttpPost]
	    [Route("accounts/{id}/set-balance")]
	    public void SetBalance(int id, AccountBalanceModel model)
	    {
	        _accountBalanceService.SetTransactionBalancesFromNow(id, model.Balance);
	    }

        [HttpGet]
        [Route("accounts/summary")]
        public IEnumerable<AccountSummaryViewModel> GetAccountsSummaries([FromUri]DateRange period)
        {
            var start = period?.Start ?? DateTime.Today.AddMonths(-1);
            var end = period?.End ?? DateTime.Today;
            return _accountSummariser.GetAccountSummaries(start, end);
        }

	    [HttpGet]
	    [Route("wealth-summary")]
	    public WealthSummaryViewModel GetAccountsSummary([FromUri]DateRange period)
	    {
	        var start = period?.Start ?? DateTime.Today.AddMonths(-1);
	        var end = period?.End ?? DateTime.Today;
	        return _accountSummariser.GetWealthSummary(start, end);
	    }

    }
}