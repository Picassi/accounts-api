using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.ViewModels.Reports;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.Services.Reports
{
    public interface ITransactionTotalSummariesProvider
    {
        IEnumerable<ReportResultsLineViewModel> GetTransactionSummaries(int id, ReportResultsQueryModel query);
    }

    public class TransactionTotalSummariesProvider : ITransactionTotalSummariesProvider
    {
        private readonly IReportGroupsProvider _reportGroupsProvider;

        public TransactionTotalSummariesProvider(IReportGroupsProvider reportGroupsProvider)
        {
            _reportGroupsProvider = reportGroupsProvider;
        }

        public IEnumerable<ReportResultsLineViewModel> GetTransactionSummaries(int id, ReportResultsQueryModel query)
        {
            var transactions = GetTransactions(id, query);

            return transactions.GroupBy(transaction => new { transaction.Category.Id, transaction.Category.Name })
                .Select(group => new ReportResultsLineViewModel
                {
                    CategoryId = @group.Key.Id,
                    Name = @group.Key.Name,
                    Amount = 
                        group.Where(x => x.ToId != null).Select(x => x.Amount).DefaultIfEmpty(0).Sum() -
                        group.Where(x => x.FromId != null).Select(x => x.Amount).DefaultIfEmpty(0).Sum()
                }).ToList();
        }

        private IQueryable<Transaction> GetTransactions(int id, ReportResultsQueryModel query)
        {
            return _reportGroupsProvider.GetReportGroups(id)
                .SelectMany(groups => groups.Categories)
                .SelectMany(categories => categories.Category.Transactions
                .Where(transaction => transaction.Date >= query.DateFrom && transaction.Date < query.DateTo));
        }
    }
}
