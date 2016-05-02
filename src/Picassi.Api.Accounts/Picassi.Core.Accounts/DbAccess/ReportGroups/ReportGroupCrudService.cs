using AutoMapper;
using Picassi.Common.Data.Services;
using Picassi.Core.Accounts.ViewModels.ReportGroups;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess.ReportGroups
{
    public interface IReportGroupCrudService
    {
        ReportGroupViewModel CreateGroup(int reportId, ReportGroupViewModel group);
        ReportGroupViewModel GetGroup(int reportId, int id);
        ReportGroupViewModel UpdateGroup(int reportId, int id, ReportGroupViewModel group);
        bool DeleteGroup(int reportId, int id);        
    }

    public class ReportGroupCrudService : IReportGroupCrudService
    {
        private readonly IAccountsDataContext _dbContext;
        private readonly IReportGroupCategoryCompiler _reportGroupCategoryCompiler;

        public ReportGroupCrudService(IAccountsDataContext dataContext, IReportGroupCategoryCompiler reportGroupCategoryCompiler)
        {
            _dbContext = dataContext;
            _reportGroupCategoryCompiler = reportGroupCategoryCompiler;
        }

        public ReportGroupViewModel CreateGroup(int reportId, ReportGroupViewModel group)
        {
            var dataModel = Mapper.Map<ReportGroup>(group);
            _dbContext.ReportGroups.Add(dataModel);
            var categories = _reportGroupCategoryCompiler.CompileReportLines(group.Id, group.CategoryIds);
            _dbContext.SyncManyToManyRelationship(dataModel, d => d.Categories, categories);
            _dbContext.SyncManyToManyRelationship(dataModel, d => d.Reports, new [] { reportId });
            _dbContext.SaveChanges();
            return Mapper.Map<ReportGroupViewModel>(dataModel);
        }

        public ReportGroupViewModel GetGroup(int reportId, int id)
        {
            var dataModel = _dbContext.ReportGroups.Find(id);
            var model = Mapper.Map<ReportGroupViewModel>(dataModel);
            model.CategoryIds = _reportGroupCategoryCompiler.GetCategories(dataModel.Categories);
            return model;
        }

        public ReportGroupViewModel UpdateGroup(int reportId, int id, ReportGroupViewModel group)
        {
            var dataModel = _dbContext.ReportGroups.Find(id);
            Mapper.Map(group, dataModel);
            var categories = _reportGroupCategoryCompiler.CompileReportLines(group.Id, group.CategoryIds);
            _dbContext.SyncManyToManyRelationship(dataModel, d => d.Categories, categories);
            _dbContext.SyncManyToManyRelationship(dataModel, d => d.Reports, new[] { reportId });
            _dbContext.SaveChanges();
            return Mapper.Map<ReportGroupViewModel>(dataModel);
        }

        public bool DeleteGroup(int reportId, int id)
        {
            var dataModel = _dbContext.ReportGroups.Find(id);
            _dbContext.ReportGroups.Remove(dataModel);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
