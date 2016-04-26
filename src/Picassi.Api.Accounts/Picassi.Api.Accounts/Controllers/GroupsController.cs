using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Common.Api.Attributes;
using Picassi.Core.Accounts.DbAccess.Groups;
using Picassi.Core.Accounts.ViewModels.Groups;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class GroupsController : ApiController
    {
        private readonly IGroupCrudService _crudService;
        private readonly IGroupQueryService _queryService;

        public GroupsController(IGroupCrudService crudService, IGroupQueryService queryService)
        {
            _crudService = crudService;
            _queryService = queryService;
        }

        [HttpGet]
        [Route("groups")]
        public IEnumerable<GroupViewModel> GetGroups([FromUri]GroupsQueryModel query)
        {
            return _queryService.Query(query);
        }

        [HttpPost]
        [Route("groups")]
        public GroupViewModel CreateGroup([FromBody]GroupViewModel GroupViewModel)
        {
            return _crudService.CreateGroup(GroupViewModel);
        }

        [HttpGet]
        [Route("groups/{id}")]
        public GroupViewModel GetGroup(int id)
        {
            return _crudService.GetGroup(id);
        }

        [HttpPut]
        [Route("groups/{id}")]
        public GroupViewModel UpdateGroup(int id, [FromBody]GroupViewModel GroupViewModel)
        {
            return _crudService.UpdateGroup(id, GroupViewModel);
        }

        [HttpDelete]
        [Route("groups/{id}")]
        public bool DeleteAccount(int id)
        {
            return _crudService.DeleteGroup(id);
        }
    }
}