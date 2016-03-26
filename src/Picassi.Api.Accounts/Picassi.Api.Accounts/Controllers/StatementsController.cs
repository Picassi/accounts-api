using System;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Common.Api.Attributes;
using Picassi.Core.Accounts.Reports;
using Picassi.Core.Accounts.ViewModels.Accounts;
using Picassi.Core.Accounts.ViewModels.Statements;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class StatementController : ApiController
    {
        private readonly IStatementService _statementService;

        public StatementController(IStatementService statementService)
        {
            _statementService = statementService;
        }

        [HttpGet]
        [Route("accounts/{accountId}/statement")]
        public StatementViewModel GetTransactions(int accountId, [FromUri]StatementQueryModel query)
        {
            return _statementService.GetStatement(accountId, query);
        }
    }
}