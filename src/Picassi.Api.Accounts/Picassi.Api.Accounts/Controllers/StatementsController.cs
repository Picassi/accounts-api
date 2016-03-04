using System;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Core.Accounts.Reports;
using Picassi.Core.Accounts.ViewModels.Accounts;
using Picassi.Core.Accounts.ViewModels.Statements;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StatementController : ApiController
    {
        private readonly IStatementService _statementService;

        public StatementController(IStatementService statementService)
        {
            _statementService = statementService;
        }

        [HttpGet]
        [Route("accounts/{accountId}/statement/{groupBy}")]
        public StatementViewModel GetTransactions([FromUri]AccountPeriodViewModel query, string groupBy)
        {
            return _statementService.GetStatement(query, groupBy);
        }
    }
}