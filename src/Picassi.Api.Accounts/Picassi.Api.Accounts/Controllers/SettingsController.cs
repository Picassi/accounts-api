using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Core.Accounts.DAL.Services;

namespace Picassi.Api.Accounts.Controllers
{
	[EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]    
    public class SettingssController : ApiController
	{
	    private readonly IAccountDataService _dataService;

	    public SettingssController(IAccountDataService dataService)
	    {
	        _dataService = dataService;
	    }

        [HttpGet]
        [Route("settings")]
	    public SettingsViewModel GetSettings()
        {
            var defaultAccount = _dataService.Query(null).SingleOrDefault(a => a.IsDefault);

            return new SettingsViewModel
            {
                DefaultAccountId = defaultAccount?.Id
            };
        }
    }

    public class SettingsViewModel
    {
        public int? DefaultAccountId { get; set; }
    }
}