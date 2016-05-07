using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Common.Api.Attributes;
using Picassi.Core.Accounts.DbAccess.Reports;
using Picassi.Core.Accounts.Services.Reports;
using Picassi.Core.Accounts.ViewModels.Reports;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class ReportsController : ApiController
    {
        private readonly IReportCrudService _crudService;
        private readonly IReportQueryService _queryService;
        private readonly IReportResultsService _resultsService;

        public ReportsController(IReportCrudService crudService, IReportQueryService queryService, IReportResultsService resultsService)
        {
            _crudService = crudService;
            _queryService = queryService;
            _resultsService = resultsService;
        }

        [HttpGet]
        [Route("reports")]
        public IEnumerable<ReportViewModel> GetReports([FromUri]ReportQueryModel query)
        {
            return _queryService.Query(query);
        }

        [HttpPost]
        [Route("reports")]
        public ReportDefinitionViewModel CreateReport([FromBody]ReportDefinitionViewModel reportViewModel)
        {
            return _crudService.CreateReport(reportViewModel);
        }

        [HttpGet]
        [Route("reports/{id}")]
        public ReportDefinitionViewModel GetReport(int id)
        {
            return _crudService.GetReport(id);
        }

        [HttpPut]
        [Route("reports/{id}")]
        public ReportDefinitionViewModel UpdateReport(int id, [FromBody]ReportDefinitionViewModel reportViewModel)
        {
            return _crudService.UpdateReport(id, reportViewModel);
        }

        [HttpDelete]
        [Route("reports/{id}")]
        public bool DeleteReport(int id)
        {
            return _crudService.DeleteReport(id);
        }

        [HttpGet]
        [Route("reports/{id}/results")]
        public ReportResultsViewModel GetReportResults(int id, [FromUri]ReportResultsQueryModel query)
        {
            return _resultsService.GetResults(id, query);
        }
    }
}