using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Api.Accounts.Contract.Calendar;
using Picassi.Core.Accounts.Models.ModelledTransactions;
using Picassi.Core.Accounts.Services.Calendar;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class CalendarController : ApiController
    {
        private readonly ICalendarCompiler _calendarCompiler;
        private readonly ICalendarReportsCompiler _calendarReportsCompiler;

        public CalendarController(ICalendarCompiler calendarCompiler, ICalendarReportsCompiler calendarReportsCompiler)
        {
            _calendarCompiler = calendarCompiler;
            _calendarReportsCompiler = calendarReportsCompiler;
        }

        [HttpGet]
        [Route("calendar")]
        public CalendarViewModel GetCalendar([FromUri] CalendarQuery query)
        {
            return _calendarCompiler.Compile(query);
        }

        [HttpGet]
        [Route("reports/modelled-spending-summary")]
        public IList<TransactionCategoriesGroupedByPeriodModel> GetModelledSpendingSummary(
            [FromUri] CalendarQuery query)
        {
            return _calendarReportsCompiler.GetReport(query);
        }
    }
}