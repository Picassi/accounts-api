using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.Services.Transactions;
using Picassi.Core.Accounts.ViewModels.Reports;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.Services.Reports
{
    public interface ITransactionTotalSummariesProvider
    {
        IEnumerable<TransactionCategoryGroupingViewModel> GetTransactionSummariesForReport(int id, ReportResultsQueryModel query);
    }

    public class TransactionTotalSummariesProvider : ITransactionTotalSummariesProvider
    {
        private readonly IReportGroupsProvider _reportGroupsProvider;

        public TransactionTotalSummariesProvider(IReportGroupsProvider reportGroupsProvider)
        {
            _reportGroupsProvider = reportGroupsProvider;
        }

        public IEnumerable<TransactionCategoryGroupingViewModel> GetTransactionSummariesForReport(int id, ReportResultsQueryModel query)
        {
            var transactions = GetTransactionsForReport(id, query);
            return TransactionUtils.GroupTransactionsByCategory(transactions, query.Accounts);
        }

        private IQueryable<Transaction> GetTransactionsForReport(int reportId, ReportResultsQueryModel query)
        {
            return _reportGroupsProvider.GetReportGroups(reportId)
                .SelectMany(groups => groups.Categories)
                .SelectMany(categories => categories.Category.Transactions.Where(
                    transaction => transaction.Date >= query.DateFrom && transaction.Date < query.DateTo))
                .Distinct();
        }
    }
}
