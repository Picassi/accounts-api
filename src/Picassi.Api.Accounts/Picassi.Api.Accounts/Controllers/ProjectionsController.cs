using System;
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
        [Route("projections")]
        public bool GenerateProjection([FromBody]ProjectionGenerationParameters query)
        {
            var start = query?.Start ?? DateTime.Now;
            var end = query?.End ?? DateTime.Now.AddYears(2);
            return _accountProjector.ProjectAccounts(start, end);
        }
    }
}