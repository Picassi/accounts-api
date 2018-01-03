using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Api.Accounts.Contract.Calendar;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.ModelledTransactions;

namespace Picassi.Core.Accounts.Services.Calendar
{
    public interface ICalendarCompiler
    {
        CalendarViewModel Compile(CalendarQuery query);
    }

    public class CalendarCompiler : ICalendarCompiler
    {
        private readonly IModelledTransactionsDataService _modelledTransactionsDataService;

        public CalendarCompiler(IModelledTransactionsDataService modelledTransactionsDataService)
        {
            _modelledTransactionsDataService = modelledTransactionsDataService;
        }

        public CalendarViewModel Compile(CalendarQuery query)
        {
            var start = CalendarUtilities.GetStartOfPeriod(query.Start ?? DateTime.UtcNow, query.PanelPeriod);
            var end = CalendarUtilities.GetEndDate(start, query.PanelPeriod);
            var cells = CompileCells(start, end, query.CellPeriod);
            return CompileCalendar(start, end, query.RowPeriod, cells);
        }
        private IList<CalendarCell> CompileCells(DateTime start, DateTime end, ReportingPeriod period)
        {
            var func = GetDateIncrementFunction(period);
            return CompileCells(start, end, func).ToList();
        }

        private IEnumerable<CalendarCell> CompileCells(DateTime start, DateTime end, Func<DateTime, DateTime> iterationFunc)
        {
            var transactions = _modelledTransactionsDataService.Query(dateFrom: start, dateTo: end).Lines.ToList();

            while (start < end)
            {
                var endPeriod = iterationFunc(start);
                var transactionsForPeriod = transactions
                    .Where(trans => trans.Date >= start && trans.Date < endPeriod)
                    .Select(trans => new CalendarTransaction
                    {
                        Amount = trans.Amount,
                        Description = trans.Description ?? "(" + trans.CategoryName + ")"
                    }).ToList();

                var cell = CompileCell(start, endPeriod, transactionsForPeriod);
                start = endPeriod;
                yield return cell;
            }
        }

        private CalendarCell CompileCell(DateTime start, DateTime endPeriod, IList<CalendarTransaction> transactionModels)
        {
            return new CalendarCell
            {
                Name = start.Day.ToString(),
                Date = start,
                Transactions = transactionModels
            };
        }

        private CalendarViewModel CompileCalendar(DateTime start, DateTime end, ReportingPeriod period, IList<CalendarCell> cells)
        {
            var periodStart = CalendarUtilities.GetStartOfPeriod(start, period);
            var viewModel = new CalendarViewModel();
            while (periodStart < end)
            {
                var calendarCells = new List<CalendarCell>();
                if (start > periodStart) calendarCells.AddRange(GetPaddingCells(periodStart, start));

                var periodEnd = GetDateIncrementFunction(period)(periodStart);
                calendarCells.AddRange(cells.Where(c => c.Date >= periodStart && c.Date < periodEnd).OrderBy(c => c.Date).ToList());

                if (periodEnd > end) calendarCells.AddRange(GetPaddingCells(end, periodEnd));

                var row = new CalendarRow { Cells = calendarCells };
                periodStart = periodEnd;
                viewModel.Rows.Add(row);
            }
            return viewModel;
        }

        private static IEnumerable<CalendarCell> GetPaddingCells(DateTime start, DateTime end)
        {
            return Enumerable.Range(0, (end - start).Days).Select(i => new CalendarCell
            {
                Date = start.AddDays(i)
            });
        }

        private Func<DateTime, DateTime> GetDateIncrementFunction(ReportingPeriod period)
        {
            switch (period)
            {
                case ReportingPeriod.Day:
                    return d => d.AddDays(1);
                case ReportingPeriod.Week:
                    return d => d.AddDays(7);
                case ReportingPeriod.Month:
                    return d => d.AddMonths(1);
                case ReportingPeriod.Quarter:
                    return d => d.AddMonths(3);
                case ReportingPeriod.Year:
                    return d => d.AddYears(1);
                default:
                    throw new ArgumentOutOfRangeException(nameof(period), period, null);
            }
        }
    }

    public class CalendarUtilities
    {
        public static DateTime GetStartOfPeriod(DateTime start, ReportingPeriod period)
        {
            switch (period)
            {
                case ReportingPeriod.Day:
                    return start.Date;
                case ReportingPeriod.Week:
                    var daysDifference = (int)start.DayOfWeek - (int)DayOfWeek.Monday;
                    return start.Date.AddDays((daysDifference + 7) % 7 - 7);
                case ReportingPeriod.Month:
                    return new DateTime(start.Year, start.Month, 1);
                case ReportingPeriod.Quarter:
                    return new DateTime(start.Year, start.Month - start.Month % 3, 1);
                case ReportingPeriod.Year:
                    return new DateTime(start.Year, 1, 1);
                default:
                    throw new ArgumentOutOfRangeException(nameof(period), period, null);
            }
        }

        public static DateTime GetEndDate(DateTime start, ReportingPeriod period)
        {
            switch (period)
            {
                case ReportingPeriod.Day:
                    return start.AddDays(1);
                case ReportingPeriod.Week:
                    return start.AddDays(7);
                case ReportingPeriod.Month:
                    return start.AddMonths(1);
                case ReportingPeriod.Quarter:
                    return start.AddMonths(3);
                case ReportingPeriod.Year:
                    return start.AddMonths(1);
                default:
                    throw new ArgumentOutOfRangeException(nameof(period), period, null);
            }
        }


    }
}
