using System;
using System.Linq;
using System.Linq.Expressions;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.Services.Reports
{
    public interface IReportGroupsProvider
    {
        IQueryable<ReportGroup> GetReportGroups(int reportId);
    }

    public class ReportGroupsProvider : IReportGroupsProvider
    {
        private readonly IAccountsDataContext _dbContext;

        public ReportGroupsProvider(IAccountsDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<ReportGroup> GetReportGroups(int reportId)
        {
            return _dbContext.ReportGroups.Where(IsLinkedToReport(reportId));
        }

        private static Expression<Func<ReportGroup, bool>> IsLinkedToReport(int id)
        {
            return group => group.Reports.Any(report => report.Id == id);
        }

    }
}
