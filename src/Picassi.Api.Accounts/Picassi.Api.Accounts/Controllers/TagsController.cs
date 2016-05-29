using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Common.Api.Attributes;
using Picassi.Core.Accounts.DbAccess.Tags;
using Picassi.Core.Accounts.ViewModels.Tags;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class TagsController : ApiController
    {
        private readonly ITagCrudService _crudService;
        private readonly ITagQueryService _queryService;

        public TagsController(ITagCrudService crudService, ITagQueryService queryService)
        {
            _crudService = crudService;
            _queryService = queryService;
        }

        [HttpGet]
        [Route("tags")]
        public IEnumerable<TagViewModel> GetTags([FromUri]TagsQueryModel query)
        {
            return _queryService.Query(query);
        }

        [HttpPost]
        [Route("tags")]
        public TagViewModel CreateTag([FromBody]TagViewModel TagViewModel)
        {
            return _crudService.CreateTag(TagViewModel);
        }

        [HttpGet]
        [Route("tags/{id}")]
        public TagViewModel GetTag(int id)
        {
            return _crudService.GetTag(id);
        }

        [HttpPut]
        [Route("tags/{id}")]
        public TagViewModel UpdateTag(int id, [FromBody]TagViewModel TagViewModel)
        {
            return _crudService.UpdateTag(id, TagViewModel);
        }

        [HttpDelete]
        [Route("tags/{id}")]
        public bool DeleteAccount(int id)
        {
            return _crudService.DeleteTag(id);
        }
    }
}