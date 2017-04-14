using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Core.Accounts.DbAccess.Reports;
using Picassi.Core.Accounts.Services.Reports;
using Picassi.Core.Accounts.ViewModels;
using Picassi.Core.Accounts.ViewModels.Reports;
using Picassi.Utils.Api.Attributes;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class ReportsController : ApiController
    {
        private readonly IReportCrudService _crudService;
        private readonly IReportQueryService _queryService;
        private readonly IReportResultsServiceProvider _resultsServiceProvider;

        public ReportsController(IReportCrudService crudService, IReportQueryService queryService, IReportResultsServiceProvider resultsServiceProvider)
        {
            _crudService = crudService;
            _queryService = queryService;
            _resultsServiceProvider = resultsServiceProvider;
        }

        [HttpGet]
        [Route("reports")]
        public ResultsViewModel<ReportViewModel> GetReports([FromUri]ReportQueryModel query)
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
            return _resultsServiceProvider.GetService(query.Type).GetResults(id, query);
        }
    }
}