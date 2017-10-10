using System.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Api.Accounts.Contract.Application;
using Picassi.Core.Accounts.DAL;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]    
    public class ApplicationController : ApiController
    {
        private readonly IMigrator _migrator;

        public ApplicationController(IMigrator migrator)
        {
            _migrator = migrator;
        }

        [HttpGet]
        [Route("application/info")]
        public ApplicationInfo GetApplicationInfo()
        {
            return new ApplicationInfo
            {
                Version = ConfigurationManager.AppSettings["application-version"]
            };
        }

        [HttpPost]
        [Route("application/{userId}/tasks/upgrade")]
        public void UpgradeAppForUser(AdminUpgradeTask task)
        {
            _migrator.ApplyPendingMigrations(task.DatabaseName);
        }

        [HttpPost]
        [PicassiApiAuthorise]
        [Route("application/tasks/upgrade")]
        public void Upgrade()
        {
            _migrator.ApplyPendingMigrations();
        }
    }
}
