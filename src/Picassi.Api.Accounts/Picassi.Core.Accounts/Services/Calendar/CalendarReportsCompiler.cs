using System;
using System.Collections.Generic;
using Picassi.Api.Accounts.Contract.Calendar;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.ModelledTransactions;

namespace Picassi.Core.Accounts.Services.Calendar
{
    public interface ICalendarReportsCompiler
    {
        IList<TransactionCategoriesGroupedByPeriodModel> GetReport(CalendarQuery query);
    }

    public class CalendarReportsCompiler : ICalendarReportsCompiler
    {
        private readonly IModelledTransactionsDataService _modelledTransactionsDataService;

        public CalendarReportsCompiler(IModelledTransactionsDataService modelledTransactionsDataService)
        {
            _modelledTransactionsDataService = modelledTransactionsDataService;
        }

        public IList<TransactionCategoriesGroupedByPeriodModel> GetReport(CalendarQuery query)
        {
            var start = CalendarUtilities.GetStartOfPeriod(query?.Start ?? DateTime.UtcNow, query?.PanelPeriod ?? ReportingPeriod.Month);
            var end = CalendarUtilities.GetEndDate(start, query?.PanelPeriod ?? ReportingPeriod.Month);

            var rowStart = CalendarUtilities.GetStartOfPeriod(start, query?.RowPeriod ?? ReportingPeriod.Week);
            var rowEnd = CalendarUtilities.GetEndDate(end, query?.RowPeriod ?? ReportingPeriod.Week);

            return _modelledTransactionsDataService.QueryGrouped(rowStart, rowEnd);

        }
    }
}