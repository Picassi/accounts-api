using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.Time;
using Picassi.Core.Accounts.ViewModels.Reports;

namespace Picassi.Core.Accounts.Services.Reports
{
    public interface ITransactionAverageSummariesProvider
    {
        IEnumerable<TransactionCategoryGroupingViewModel> GetTransactionSummaries(int id, ReportResultsQueryModel query);
    }

    public class TransactionAverageSummariesProvider : ITransactionAverageSummariesProvider
    {
        private readonly ITransactionTotalSummariesProvider _transactionTotalSummariesProvider;
        private readonly IPeriodCalculator _periodCalculator;

        public TransactionAverageSummariesProvider(ITransactionTotalSummariesProvider transactionTotalSummariesProvider, IPeriodCalculator periodCalculator)
        {
            _transactionTotalSummariesProvider = transactionTotalSummariesProvider;
            _periodCalculator = periodCalculator;
        }

        public IEnumerable<TransactionCategoryGroupingViewModel> GetTransactionSummaries(int id, ReportResultsQueryModel query)
        {
            var definition = new PeriodDefinition {  Quantity = 1, Type = query.Period };
            var range = new DateRange { Start = query.DateFrom, End = query.DateTo };
            var divider = _periodCalculator.GetNumberOfPeriods(definition, range);

            return _transactionTotalSummariesProvider.GetTransactionSummariesForReport(id, query)
                .Select(summary => new TransactionCategoryGroupingViewModel
                {
                    CategoryId = summary.CategoryId,
                    Name = summary.Name,
                    Amount = Math.Round(summary.Amount / divider, 2)
                });
        }
    }
}
