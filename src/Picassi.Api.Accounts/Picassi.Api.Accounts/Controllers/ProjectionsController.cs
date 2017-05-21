using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Core.Accounts.Services.Projections;
using Picassi.Utils.Api.Attributes;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class ProjectionsController
    {
        private readonly IAccountProjector _accountProjector;

        public ProjectionsController(IAccountProjector accountProjector)
        {
            _accountProjector = accountProjector;
        }

        [HttpPost]
        [Route("project-account")]
        public bool GenerateProjection([FromBody]ProjectionGenerationParameters query)
        {
            return _accountProjector.ProjectAccounts(query);
        }
    }
}