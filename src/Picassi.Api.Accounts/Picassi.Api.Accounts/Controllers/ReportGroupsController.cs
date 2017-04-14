using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Core.Accounts.DbAccess.ReportGroups;
using Picassi.Core.Accounts.ViewModels.ReportGroups;
using Picassi.Utils.Api.Attributes;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class ReportGroupsController : ApiController
    {
        private readonly IReportGroupCrudService _crudService;
        private readonly IReportGroupQueryService _queryService;

        public ReportGroupsController(IReportGroupCrudService crudService, IReportGroupQueryService queryService)
        {
            _crudService = crudService;
            _queryService = queryService;
        }

        [HttpGet]
        [Route("reports/{reportId}/groups")]
        public IEnumerable<ReportGroupViewModel> GetReportGroups(int reportId, [FromUri]ReportGroupsQueryModel query)
        {
            return _queryService.Query(reportId, query);
        }

        [HttpPost]
        [Route("reports/{reportId}/groups")]
        public ReportGroupViewModel CreateReportGroup(int reportId, [FromBody]ReportGroupViewModel ReportGroupViewModel)
        {
            return _crudService.CreateGroup(reportId, ReportGroupViewModel);
        }

        [HttpGet]
        [Route("reports/{reportId}/groups/{id}")]
        public ReportGroupViewModel GetReportGroup(int reportId, int id)
        {
            return _crudService.GetGroup(reportId, id);
        }

        [HttpPut]
        [Route("reports/{reportId}/groups/{id}")]
        public ReportGroupViewModel UpdateReportGroup(int reportId, int id, [FromBody]ReportGroupViewModel ReportGroupViewModel)
        {
            return _crudService.UpdateGroup(reportId, id, ReportGroupViewModel);
        }

        [HttpDelete]
        [Route("reports/{reportId}/groups/{id}")]
        public bool DeleteAccount(int reportId, int id)
        {
            return _crudService.DeleteGroup(reportId, id);
        }
    }
}