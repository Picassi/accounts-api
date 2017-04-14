using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Core.Accounts.Services.Meta;
using Picassi.Core.Accounts.ViewModels.Meta;
using Picassi.Utils.Api.Attributes;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class MetaController : ApiController
    {
        private readonly IMetaDataService _metaDataService;

        public MetaController(IMetaDataService metaDataService)
        {
            _metaDataService = metaDataService;
        }

        [HttpGet]
        [Route("meta")]
        public MetaDataViewModel GetMetaData()
        {
            return _metaDataService.GetMetaData();
        }
    }
}
