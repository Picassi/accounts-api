using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Picassi.Core.Accounts.ViewModels.ReportGroups;
using Picassi.Data.Accounts.Database;

namespace Picassi.Core.Accounts.DbAccess.ReportGroups
{
    public interface IReportGroupQueryService
    {
        IEnumerable<ReportGroupViewModel> Query(int reportId, ReportGroupsQueryModel accounts);
    }

    public class ReportGroupQueryService : IReportGroupQueryService
    {
        private readonly IAccountsDataContext _dbContext;
        
        public ReportGroupQueryService(IAccountsDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        public IEnumerable<ReportGroupViewModel> Query(int reportId, ReportGroupsQueryModel accounts)
        {
            var queryResults = _dbContext.ReportGroups.Where(x => x.Reports.Any(y => y.Id == reportId));

            if (accounts?.Name != null) queryResults = queryResults.Where(x => x.Name.Contains(accounts.Name));

            return Mapper.Map<IEnumerable<ReportGroupViewModel>>(queryResults);
        }
    }
}
