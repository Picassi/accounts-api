using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Core.Accounts.DAL;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class DatabaseController : ApiController
    {
        private readonly IMigrator _migrator;

        public DatabaseController(IMigrator migrator)
        {
            _migrator = migrator;
        }

        [HttpPost]
        [Route("database/update")]
        public void Update()
        {
            _migrator.ApplyPendingMigrations();
        }
    }
}
