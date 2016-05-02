using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Common.Api.Attributes;
using Picassi.Core.Accounts.DbAccess.Reports;
using Picassi.Core.Accounts.ViewModels.Reports;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class ReportController : ApiController
    {
        private readonly IReportCrudService _crudService;
        private readonly IReportQueryService _queryService;

        public ReportController(IReportCrudService crudService, IReportQueryService queryService)
        {
            _crudService = crudService;
            _queryService = queryService;
        }

        [HttpGet]
        [Route("reports")]
        public IEnumerable<ReportViewModel> GetReports([FromUri]ReportQueryModel query)
        {
            return _queryService.Query(query);
        }

        [HttpPost]
        [Route("reports")]
        public ReportViewModel CreateReport([FromBody]ReportViewModel reportViewModel)
        {
            return _crudService.CreateReport(reportViewModel);
        }

        [HttpGet]
        [Route("reports/{id}")]
        public ReportViewModel GetReport(int id)
        {
            return _crudService.GetReport(id);
        }

        [HttpPut]
        [Route("reports/{id}")]
        public ReportViewModel UpdateReport(int id, [FromBody]ReportViewModel reportViewModel)
        {
            return _crudService.UpdateReport(id, reportViewModel);
        }

        [HttpDelete]
        [Route("reports/{id}")]
        public bool DeleteAccount(int id)
        {
            return _crudService.DeleteReport(id);
        }
    }
}