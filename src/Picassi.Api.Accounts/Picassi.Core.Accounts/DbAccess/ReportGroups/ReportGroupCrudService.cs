using System.Collections.Generic;
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
        private readonly IReportGroupReportCompiler _reportGroupReportCompiler;

        public ReportGroupCrudService(IAccountsDataContext dataContext, IReportGroupCategoryCompiler reportGroupCategoryCompiler, IReportGroupReportCompiler reportGroupReportCompiler)
        {
            _dbContext = dataContext;
            _reportGroupCategoryCompiler = reportGroupCategoryCompiler;
            _reportGroupReportCompiler = reportGroupReportCompiler;
        }

        public ReportGroupViewModel CreateGroup(int reportId, ReportGroupViewModel group)
        {
            var dataModel = Mapper.Map<ReportGroup>(group);
            dataModel.Categories = new List<ReportGroupCategory>();
            dataModel.Reports = new List<ReportGroupReport>();
            _dbContext.ReportGroups.Add(dataModel);
            _dbContext.SaveChanges();
            var categories = _reportGroupCategoryCompiler.CompileReportLines(dataModel.Id, group.CategoryIds);
            _dbContext.SyncManyToManyRelationship(dataModel, d => d.Categories, categories);
            var reports = _reportGroupReportCompiler.CompileReportLines(dataModel.Id, new List<int> { reportId });
            _dbContext.SyncManyToManyRelationship(dataModel, d => d.Reports, reports);
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
            var reports = _reportGroupReportCompiler.CompileReportLines(group.Id, new List<int> { reportId });
            _dbContext.SyncManyToManyRelationship(dataModel, d => d.Reports, reports);
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
