using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Api.Accounts.Contract;
using Picassi.Core.Accounts.Models;
using Picassi.Core.Accounts.Services.Charts;
using Picassi.Utils.Api.Attributes;

namespace Picassi.Api.Accounts.Controllers
{
	[EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class ChartsController : ApiController
	{
	    private readonly IChartCompiler _chartCompiler;

	    public ChartsController(IChartCompiler chartCompiler)
	    {
	        _chartCompiler = chartCompiler;
	    }

	    [HttpPost]
        [Route("charts/account-balance")]
        public LineChartModel GetAccounts([FromBody]AccountBalanceChartRequest query)
        {
            var dataPoints = _chartCompiler.GetAccountBalanceDataSeries(query.DateFrom, query.DateTo).ToList();
            return new LineChartModel
            {
                Labels = dataPoints[0].Data.Select(x => x[0].ToString()).ToList(),
                Series = dataPoints,
                Type = "spline"
            };
        }

	    [HttpPost]
	    [Route("charts/spending-by-category")]
	    public PieChartModel GetSpendingByCategory([FromBody]AccountBalanceChartRequest query)
	    {
	        var data = _chartCompiler.GetSpendingByCategoryDataSeries(query.DateFrom, query.DateTo);
	        return new PieChartModel
	        {
	            Series = data
	        };
	    }

	    [HttpPost]
	    [Route("charts/spending-versus-budget")]
	    public LineChartModel GetSpendingVersusBudget([FromBody]AccountBalanceChartRequest query)
	    {
            throw new NotImplementedException();
	    }
    }
}