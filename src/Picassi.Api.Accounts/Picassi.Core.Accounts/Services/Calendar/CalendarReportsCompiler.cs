using System;
using System.Collections.Generic;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.ModelledTransactions;
using Picassi.Core.Accounts.Services.Projections;

namespace Picassi.Core.Accounts.Services.Calendar
{
    public interface ICalendarReportsCompiler
    {
        IList<TransactionCategoriesGroupedByPeriodModel> GetReport(CalendarReportsQuery query);
    }

    public class CalendarReportsCompiler : ICalendarReportsCompiler
    {
        private readonly IModelledTransactionsDataService _modelledTransactionsDataService;

        public CalendarReportsCompiler(IModelledTransactionsDataService modelledTransactionsDataService)
        {
            _modelledTransactionsDataService = modelledTransactionsDataService;
        }

        public IList<TransactionCategoriesGroupedByPeriodModel> GetReport(CalendarReportsQuery query)
        {
            return _modelledTransactionsDataService.QueryGrouped(
                new ModelledTransactionQueryModel
                {
                    DateFrom = query.Start,
                    DateTo = query.End
                });
        }
    }

    public class CalendarReportsQuery
    {
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }
}