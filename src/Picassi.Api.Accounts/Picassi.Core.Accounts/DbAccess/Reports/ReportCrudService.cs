using AutoMapper;
using Picassi.Core.Accounts.ViewModels.Reports;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess.Reports
{
    public interface IReportCrudService
    {
        ReportViewModel CreateReport(ReportViewModel report);
        ReportViewModel GetReport(int id);
        ReportViewModel UpdateReport(int id, ReportViewModel report);
        bool DeleteReport(int id);
    }

    public class ReportCrudService : IReportCrudService
    {
        private readonly IAccountsDataContext _dbContext;

        public ReportCrudService(IAccountsDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        public ReportViewModel CreateReport(ReportViewModel report)
        {
            var dataModel = Mapper.Map<Report>(report);
            _dbContext.Reports.Add(dataModel);
            _dbContext.SaveChanges();
            return Mapper.Map<ReportViewModel>(dataModel);
        }

        public ReportViewModel GetReport(int id)
        {
            var dataModel = _dbContext.Reports.Find(id);
            return Mapper.Map<ReportViewModel>(dataModel);
        }

        public ReportViewModel UpdateReport(int id, ReportViewModel report)
        {
            var dataModel = _dbContext.Reports.Find(id);
            Mapper.Map(report, dataModel);
            _dbContext.SaveChanges();
            return Mapper.Map<ReportViewModel>(dataModel);
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
