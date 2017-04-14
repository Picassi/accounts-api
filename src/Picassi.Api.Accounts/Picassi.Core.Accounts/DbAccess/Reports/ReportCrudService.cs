using AutoMapper;
using Picassi.Core.Accounts.ViewModels.Reports;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;
using Picassi.Utils.Data.Services;

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
        private readonly IReportGroupReportCompiler _reportGroupReportCompiler;

        public ReportCrudService(IAccountsDataContext dataContext, IReportGroupReportCompiler reportGroupReportCompiler)
        {
            _dbContext = dataContext;
            _reportGroupReportCompiler = reportGroupReportCompiler;
        }

        public ReportDefinitionViewModel CreateReport(ReportDefinitionViewModel report)
        {
            var dataModel = Mapper.Map<Report>(report);
            _dbContext.Reports.Add(dataModel);
            var groups = _reportGroupReportCompiler.CompileReportLines(report.Id, report.GroupIds);
            _dbContext.SyncManyToManyRelationship(dataModel, d => d.ReportGroups, groups);
            _dbContext.SaveChanges();
            return GetReport(dataModel.Id);
        }

        public ReportDefinitionViewModel GetReport(int id)
        {
            var dataModel = _dbContext.Reports.Find(id);
            return Mapper.Map<ReportDefinitionViewModel>(dataModel);
        }

        public ReportDefinitionViewModel UpdateReport(int id, ReportDefinitionViewModel report)
        {
            var dataModel = _dbContext.Reports.Find(id);
            Mapper.Map(report, dataModel);
            var groups = _reportGroupReportCompiler.CompileReportLines(report.Id, report.GroupIds);
            _dbContext.SyncManyToManyRelationship(dataModel, d => d.ReportGroups, groups);
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
    }
}
