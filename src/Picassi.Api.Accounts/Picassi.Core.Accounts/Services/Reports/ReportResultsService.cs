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
            var reportLines = GetReportLines(query);
            var data = _dbContext.ReportGroupReports.Where(x => x.ReportId == id)
                .Select(group => new
                {
                    Name = group.ReportGroup.Name,
                    Categories = group.ReportGroup.Categories.Select(x => x.CategoryId),
                })
                .ToList();
            return data
                .Select(group => new ReportResultsGroupViewModel
                {
                    Title = group.Name,
                    ReportLines = reportLines.Where(line => group.Categories.Contains(line.CategoryId)).ToList()
                })
                .ToList();
        }

        private IEnumerable<ReportResultsLineViewModel> GetReportLines(ReportResultsQueryModel query)
        {
            var matchingDebits = _dbContext.Transactions.Where(x => x.Date >= query.DateFrom && x.Date < query.DateTo && x.FromId != null);
            var matchingCredits = _dbContext.Transactions.Where(x => x.Date >= query.DateFrom && x.Date < query.DateTo && x.ToId != null);

            /*if (query.Accounts != null)
            {
                matchingDebits = matchingDebits.Where(x => query.Accounts.Contains((int)x.FromId));
                matchingCredits = matchingDebits.Where(x => query.Accounts.Contains((int)x.ToId));
            } */               

            var summaryAmounts = _dbContext.Categories.Select(category => new
            {
                CategoryId = category.Id,
                Name = category.Name,
                Credit = matchingCredits.Where(transaction => transaction.CategoryId == category.Id)
                    .Select(x => x.Amount).DefaultIfEmpty(0).Sum(),
                Debit = matchingDebits.Where(transaction => transaction.CategoryId == category.Id)
                    .Select(x => x.Amount).DefaultIfEmpty(0).Sum()
            });

            return summaryAmounts.Select(summary => new ReportResultsLineViewModel
            {
                CategoryId = summary.CategoryId,
                Name = summary.Name,
                Amount = summary.Credit - summary.Debit
            }).ToList();
        }
    }
}
