using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Picassi.Core.Accounts.ViewModels.Reports;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.Services.Reports
{
    public interface IReportResultsService
    {
        ReportResultsViewModel GetResults(int id, ReportResultsQueryModel query);
    }

    public class ReportResultsService : IReportResultsService
    {
        private readonly IAccountsDataContext _dbContext;

        public ReportResultsService(IAccountsDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ReportResultsViewModel GetResults(int id, ReportResultsQueryModel query)
        {
            return new ReportResultsViewModel
            {
                Id = id,
                ReportGroups = GetReportGroups(id, query)
            };
        }

        private List<ReportResultsGroupViewModel> GetReportGroups(int id, ReportResultsQueryModel query)
        {
            return GetReportLinks(id).Select(link => new ReportResultsGroupViewModel()).ToList();
        }
        
        private IQueryable<ReportGroupReport> GetReportLinks(int id)
        {
            return _dbContext.ReportGroupReports.Where(link => link.ReportId == id).OrderBy(link => link.Ordinal);
        } 


    }
}
