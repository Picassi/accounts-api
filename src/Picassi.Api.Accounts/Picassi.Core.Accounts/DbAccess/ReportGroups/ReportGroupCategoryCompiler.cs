using System.Collections.Generic;
using System.Linq;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess.ReportGroups
{
    public interface IReportGroupCategoryCompiler
    {
        List<int> GetCategories(ICollection<ReportGroupCategory> reportLines);
        IEnumerable<int> CompileReportLines(int groupId, List<int> categoryIds);
    }

    public class ReportGroupCategoryCompiler : IReportGroupCategoryCompiler
    {
        private readonly IAccountsDataContext _dataContext;

        public ReportGroupCategoryCompiler(IAccountsDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<int> GetCategories(ICollection<ReportGroupCategory> reportLines)
        {
            return reportLines.OrderBy(x => x.Ordinal).Select(x => x.CategoryId).ToList();
        }

        public IEnumerable<int> CompileReportLines(int groupId, List<int> categoryIds)
        {
            if (categoryIds == null) return new List<int>();

            var categoryLines = categoryIds.Select((t, i) => GetReportLine(groupId, t, i)).ToList();
            _dataContext.SaveChanges();
            return categoryLines.Select(x => x.Id);
        }

        private ReportGroupCategory GetReportLine(int groupId, int categoryId, int index)
        {
            var reportLine = GetExisting(groupId, categoryId) ?? GetNew(groupId, categoryId);
            reportLine.Ordinal = index;
            return reportLine;
        }

        private ReportGroupCategory GetExisting(int groupId, int categoryId)
        {
            return _dataContext.ReportGroupCategories.FirstOrDefault(x => x.GroupId == groupId && x.CategoryId == categoryId);
        }

        private ReportGroupCategory GetNew(int groupId, int categoryId)
        {
            var reportCategory = new ReportGroupCategory { CategoryId = categoryId, GroupId = groupId };
            _dataContext.ReportGroupCategories.Add(reportCategory);
            return reportCategory;
        }
    }
}
