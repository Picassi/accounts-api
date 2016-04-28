using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Common.Api.Attributes;
using Picassi.Core.Accounts.ViewModels.Groups;
using Picassi.Core.Accounts.ViewModels.Reports;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class ReportsController : ApiController
    {
        [HttpGet]
        [Route("report")]
        public IEnumerable<GroupViewModel> GetReportDefinitions([FromUri]GroupsQueryModel query)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("reports/{reportId}")]
        public ReportViewModel GetReport(int reportId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("reports")]
        public ReportViewModel CreateReport([FromBody]ReportViewModel report)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("reports/{reportId}")]
        public ReportViewModel UpdateReport(int reportId, [FromBody]ReportViewModel report)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("reports/{reportId}")]
        public bool DeleteReport(int reportId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("reports/{reportId}/results")]
        public bool GetReportResults(int reportId, [FromUri]ReportResultsQueryModel query)
        {
            throw new NotImplementedException();
        }
    }
}