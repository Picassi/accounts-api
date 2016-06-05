using System.Collections.Generic;
using System.Linq;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess.Reports
{
    public interface IReportGroupReportCompiler
    {
        List<int> GetReports(ICollection<ReportGroupReport> reportLines);
        IEnumerable<int> CompileReportLines(int groupId, List<int> reportIds);
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

        public IEnumerable<int> CompileReportLines(int groupId, List<int> reportIds)
        {
            if (reportIds == null) return new List<int>();

            var reportLines = reportIds.Select((t, i) => GetReportLine(groupId, t, i)).ToList();
            _dataContext.SaveChanges();
            return reportLines.Select(x => x.Id).ToList();
        }

        private ReportGroupReport GetReportLine(int groupId, int reportId, int index)
        {
            var reportLine = GetExisting(groupId, reportId) ?? GetNew(groupId, reportId);
            reportLine.Ordinal = index;
            return reportLine;
        }

        private ReportGroupReport GetExisting(int groupId, int reportId)
        {
            return _dataContext.ReportGroupReports.FirstOrDefault(x => x.ReportGroupId == groupId && x.ReportId == reportId);
        }

        private ReportGroupReport GetNew(int groupId, int reportId)
        {
            return new ReportGroupReport { ReportId = reportId, ReportGroupId = groupId };
        }
    }
}
