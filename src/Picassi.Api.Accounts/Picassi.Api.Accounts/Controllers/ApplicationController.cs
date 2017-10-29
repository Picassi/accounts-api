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
        private readonly IDbVersionProvider _dbVersionProvider;

        public ApplicationController(IMigrator migrator, IDbVersionProvider dbVersionProvider)
        {
            _migrator = migrator;
            _dbVersionProvider = dbVersionProvider;
        }

        [HttpGet]
        [Route("application/info")]
        public ApplicationInfo GetApplicationInfo()
        {                    
            return new ApplicationInfo
            {
                Version = _dbVersionProvider.GetLatestVersion()
            };
        }

        [HttpPost]
        [Route("application/{userId}/tasks/upgrade")]
        public ApplicationInfo UpgradeAppForUser(AdminUpgradeTask task)
        {
            _migrator.ApplyPendingMigrations(task.DatabaseName);

            return new ApplicationInfo
            {
                Version = _dbVersionProvider.GetVersionForDatabase(task.DatabaseName)
            };
        }

        [HttpPost]
        [PicassiApiAuthorise]
        [Route("application/tasks/upgrade")]
        public ApplicationInfo Upgrade()
        {
            _migrator.ApplyPendingMigrations();

            return new ApplicationInfo
            {
                Version = _dbVersionProvider.GetVersionForActiveUser()
            };
        }
    }
}
