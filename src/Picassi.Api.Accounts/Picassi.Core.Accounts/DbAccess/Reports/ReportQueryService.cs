using System.Collections.Generic;
using AutoMapper;
using Picassi.Core.Accounts.ViewModels.Reports;
using Picassi.Data.Accounts.Database;

namespace Picassi.Core.Accounts.DbAccess.Reports
{
    public interface IReportQueryService
    {
        IEnumerable<ReportViewModel> Query(ReportQueryModel reports);
    }

    public class ReportQueryService : IReportQueryService
    {
        private readonly IAccountsDataContext _dbContext;
        
        public ReportQueryService(IAccountsDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        public IEnumerable<ReportViewModel> Query(ReportQueryModel reports)
        {			
            var queryResults = _dbContext.Reports; 
			return Mapper.Map<IEnumerable<ReportViewModel>>(queryResults);
        }
    }
}
