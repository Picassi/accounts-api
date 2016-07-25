using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Picassi.Core.Accounts.ViewModels;
using Picassi.Core.Accounts.ViewModels.Reports;
using Picassi.Data.Accounts.Database;

namespace Picassi.Core.Accounts.DbAccess.Reports
{
    public interface IReportQueryService
    {
        ResultsViewModel<ReportViewModel> Query(ReportQueryModel reports);
    }

    public class ReportQueryService : IReportQueryService
    {
        private readonly IAccountsDataContext _dbContext;
        
        public ReportQueryService(IAccountsDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        public ResultsViewModel<ReportViewModel> Query(ReportQueryModel reports)
        {			
            var queryResults = _dbContext.Reports;
            var results = Mapper.Map<IEnumerable<ReportViewModel>>(queryResults).ToList();
            return new ResultsViewModel<ReportViewModel>
            {
                Lines = results,
                TotalLines = results.Count
            };
        }
    }
}
