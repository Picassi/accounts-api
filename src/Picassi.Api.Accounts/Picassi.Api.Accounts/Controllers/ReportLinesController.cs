using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Common.Api.Attributes;
using Picassi.Core.Accounts.ViewModels.Reports;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class ReportLinesController : ApiController
    {
        [HttpGet]
        [Route("reports/{reportId}/lines")]
        public IEnumerable<ReportLineDefinitionViewModel> GetReportLines(int reportId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("reports/{reportId}/lines/{reportLineId}")]
        public ReportLineDefinitionViewModel GetReporLine(int reportId, int reportLineId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("reports/{reportId}/lines")]
        public ReportLineDefinitionViewModel GetReporLinetDefinition(int reportId, [FromBody]ReportLineDefinitionViewModel line)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("reports/{reportId}/lines/{reportLineId}")]
        public ReportLineDefinitionViewModel UpdateReportLine(int reportId, int reportLineId, [FromBody]ReportLineDefinitionViewModel line)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("reports/{reportId}/lines/{reportLineId}")]
        public bool DeleteReportLine(int reportId, int reportLineId)
        {
            throw new NotImplementedException();
        }
    }
}