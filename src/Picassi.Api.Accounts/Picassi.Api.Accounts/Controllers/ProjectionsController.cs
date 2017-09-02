using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Core.Accounts.Services.Projections;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class ProjectionsController : ApiController
    {
        private readonly IAccountProjector _accountProjector;

        public ProjectionsController(IAccountProjector accountProjector)
        {
            _accountProjector = accountProjector;
        }

        [HttpPost]
        [Route("accounts/project")]
        public bool GenerateProjection([FromBody]ProjectionGenerationParameters query)
        {
            return _accountProjector.ProjectAccounts(query);
        }

        [HttpPost]
        [Route("accounts/{id}/project")]
        public bool GenerateProjectionForAccount(int id, [FromBody]ProjectionGenerationParameters query)
        {
            return _accountProjector.ProjectAccounts(query);
        }

    }
}