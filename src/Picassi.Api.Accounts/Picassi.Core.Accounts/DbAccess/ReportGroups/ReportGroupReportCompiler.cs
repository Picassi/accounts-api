using System.Collections.Generic;
using System.Linq;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess.ReportGroups
{
    public interface IReportGroupReportCompiler
    {
        List<int> GetReports(ICollection<ReportGroupReport> reportLines);
        IEnumerable<int> CompileReportLines(int groupId, List<int> categoryIds);
    }

    public class ReportGroupReportCompiler : IReportGroupReportCompiler
    {
        private readonly IAccountsDataContext _dataContext;

        public ReportGroupReportCompiler(IAccountsDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<int> GetReports(ICollection<ReportGroupReport> reportLines)
        {
            return reportLines.OrderBy(x => x.Ordinal).Select(x => x.ReportId).ToList();
        }

        public IEnumerable<int> CompileReportLines(int groupId, List<int> categoryIds)
        {
            if (categoryIds == null) return new List<int>();

            var categoryLines = categoryIds.Select((t, i) => GetReportLine(groupId, t, i)).ToList();
            _dataContext.SaveChanges();
            return categoryLines.Select(x => x.Id);
        }

        private ReportGroupReport GetReportLine(int groupId, int categoryId, int index)
        {
            var reportLine = GetExisting(groupId, categoryId) ?? GetNew(groupId, categoryId);
            reportLine.Ordinal = index;
            return reportLine;
        }

        private ReportGroupReport GetExisting(int groupId, int categoryId)
        {
            return _dataContext.ReportGroupReports.FirstOrDefault(x => x.GroupId == groupId && x.ReportId == categoryId);
        }

        private ReportGroupReport GetNew(int groupId, int categoryId)
        {
            var reportGroupReport = new ReportGroupReport { ReportId = categoryId, GroupId = groupId };
            _dataContext.ReportGroupReports.Add(reportGroupReport);
            return reportGroupReport;
        }
    }
}
