using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Picassi.Common.Data.Services;
using Picassi.Core.Accounts.ViewModels.Reports;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess.Reports
{
    public interface IReportCrudService
    {
        ReportDefinitionViewModel CreateReport(ReportDefinitionViewModel report);
        ReportDefinitionViewModel GetReport(int id);
        ReportDefinitionViewModel UpdateReport(int id, ReportDefinitionViewModel report);
        bool DeleteReport(int id);
    }

    public class ReportCrudService : IReportCrudService
    {
        private readonly IAccountsDataContext _dbContext;

        public ReportCrudService(IAccountsDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        public ReportDefinitionViewModel CreateReport(ReportDefinitionViewModel report)
        {
            var dataModel = Mapper.Map<Report>(report);

            _dbContext.Reports.Add(dataModel);
            _dbContext.SaveChanges();

            _dbContext.SyncManyToManyRelationship(dataModel, d => d.ReportLines, SetReportLinesInDbAndReturn(report).Select(x => x.Id));
            _dbContext.SaveChanges();

            return GetReport(dataModel.Id);
        }

        public ReportDefinitionViewModel GetReport(int id)
        {
            var dataModel = _dbContext.Reports.Find(id);
            var definition = Mapper.Map<ReportDefinitionViewModel>(dataModel);
            definition.Groups = GetReportDefinitionGroups(id);
            definition.Groups.Add(GetUngroupedCategoriesForReport(id));
            return definition;
        }

        public ReportDefinitionViewModel UpdateReport(int id, ReportDefinitionViewModel report)
        {
            var dataModel = _dbContext.Reports.Find(id);
            Mapper.Map(report, dataModel);

            _dbContext.SyncManyToManyRelationship(dataModel, d => d.ReportLines, SetReportLinesInDbAndReturn(report).Select(x => x.Id));
            _dbContext.SaveChanges();

            return Mapper.Map<ReportDefinitionViewModel>(dataModel);
        }

        public bool DeleteReport(int id)
        {
            var dataModel = _dbContext.Reports.Find(id);
            _dbContext.Reports.Remove(dataModel);
            _dbContext.SaveChanges();
            return true;
        }

        private IEnumerable<ReportLineDefinition> SetReportLinesInDbAndReturn(ReportDefinitionViewModel report)
        {
            var reportLines = _dbContext.Categories.Select(category => GetReportLineForCategory(report, category)).Where(reportLine => reportLine != null).ToList();
            _dbContext.SaveChanges();
            return reportLines;
        }

        private ReportLineDefinition GetReportLineForCategory(ReportDefinitionViewModel report, Category category)
        {
            var existingLine = _dbContext.ReportLines.FirstOrDefault(x => x.ReportId == report.Id && x.CategoryId == category.Id);
            var categoryGroup = report.Groups.FirstOrDefault(x => x.Categories.Any(y => y.Id == category.Id));

            if (categoryGroup?.Id == null)
            {
                return RemoveReportLineDefinitionIfExists(existingLine);
            }

            return existingLine == null 
                ? CraeteNewReportLineDefinition(report, category, categoryGroup) 
                : UpdateReportLineDefinition(existingLine, categoryGroup);
        }

        private static ReportLineDefinition UpdateReportLineDefinition(ReportLineDefinition existingLine,
            ReportGroupViewModel categoryGroup)
        {
            existingLine.GroupId = categoryGroup.Id;
            return existingLine;
        }

        private ReportLineDefinition CraeteNewReportLineDefinition(ReportDefinitionViewModel report, Category category, ReportGroupViewModel categoryGroup)
        {
            var existingLine = new ReportLineDefinition
            {
                ReportId = report.Id,
                CategoryId = category.Id,
                GroupId = categoryGroup.Id
            };
            _dbContext.ReportLines.Add(existingLine);
            return existingLine;
        }

        private ReportLineDefinition RemoveReportLineDefinitionIfExists(ReportLineDefinition existingLine)
        {
            if (existingLine != null)
            {
                _dbContext.ReportLines.Remove(existingLine);
            }
            return null;
        }

        private ReportGroupViewModel GetUngroupedCategoriesForReport(int id)
        {
            var categoryIds = _dbContext.ReportLines.Where(x => x.ReportId == id).Select(x => x.CategoryId);
            return new ReportGroupViewModel
            {
                Id = null,
                Name = "Unused",
                Editable = false,
                Categories = _dbContext.Categories
                    .Where(x => !categoryIds.Contains(x.Id))
                    .Select(x => new ReportGroupCategoryViewModel
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList()
            };
        }

        private List<ReportGroupViewModel> GetReportDefinitionGroups(int id)
        {
            return _dbContext.ReportLines
                .Where(x => x.ReportId == id)
                .GroupBy(x => new { Id = x.GroupId, Name = x.Group.Name })
                .Select(x => new ReportGroupViewModel
                {
                    Id = x.Key.Id,
                    Name = x.Key.Name,
                    Editable = true,
                    Categories = x.Select(y => new ReportGroupCategoryViewModel { Id = y.Category.Id, Name = y.Category.Name }).ToList()
                }).ToList();
        }
    }
}
