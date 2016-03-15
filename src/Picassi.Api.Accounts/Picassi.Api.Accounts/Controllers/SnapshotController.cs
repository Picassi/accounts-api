using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Common.Api.Attributes;
using Picassi.Core.Accounts.DbAccess.Snapshots;
using Picassi.Core.Accounts.ViewModels.Snapshots;

namespace Picassi.Api.Accounts.Controllers
{
	[EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class SnapshotController : ApiController
    {
        private readonly ISnapshotCrudService _crudService;
        private readonly ISnapshotQueryService _queryService;

        public SnapshotController(ISnapshotCrudService crudService, ISnapshotQueryService queryService)
        {
            _crudService = crudService;
            _queryService = queryService;
        }

        [HttpGet]
        [Route("snapshots")]
        public IEnumerable<SnapshotViewModel> GetSnapshots([FromUri]SnapshotQueryModel query)
        {
            return _queryService.Query(query);
        }

        [HttpPost]
        [Route("snapshots")]
        public SnapshotViewModel CreateSnapshot([FromBody]SnapshotViewModel snapshotViewModel)
        {
            return _crudService.CreateSnapshot(snapshotViewModel);
        }

        [HttpGet]
        [Route("snapshots/{id}")]
        public SnapshotViewModel GetSnapshot(int id)
        {
            return _crudService.GetSnapshot(id);            
        }

        [HttpPut]
        [Route("snapshots/{id}")]
        public SnapshotViewModel UpdateSnapshot(int id, [FromBody]SnapshotViewModel snapshotViewModel)
        {
            return _crudService.UpdateSnapshot(id, snapshotViewModel);
        }

        [HttpDelete]
        [Route("snapshots/{id}")]
        public bool DeleteAccount(int id)
        {
            return _crudService.DeleteSnapshot(id);
        }
	}
}