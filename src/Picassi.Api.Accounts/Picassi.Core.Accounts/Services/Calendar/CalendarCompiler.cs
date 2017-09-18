using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Api.Accounts.Contract.Calendar;

namespace Picassi.Core.Accounts.Services.Calendar
{
    public interface ICalendarCompiler
    {
        CalendarViewModel Compile(CalendarQuery query);
    }

    public class CalendarCompiler : ICalendarCompiler
    {
        public CalendarViewModel Compile(CalendarQuery query)
        {
            var start = GetStartOfPeriod(query.Start ?? DateTime.UtcNow, query.PanelPeriod);
            var end = GetEndDate(start, query.PanelPeriod);
            var cells = CompileCells(start, end, query.CellPeriod);
            return CompileCalendar(start, end, query.RowPeriod, cells);
        }

        private DateTime GetEndDate(DateTime start, ReportingPeriod period)
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

        private IList<CalendarCell> CompileCells(DateTime start, DateTime end, ReportingPeriod period)
        {
            var func = GetDateIncrementFunction(period);
            return CompileCells(start, end, func).ToList();
        }

        private IEnumerable<CalendarCell> CompileCells(DateTime start, DateTime end, Func<DateTime, DateTime> iterationFunc)
        {
            while (start < end)
            {
                var endPeriod = iterationFunc(start);
                start = endPeriod;
                yield return CompileCell(start, endPeriod);
            }
        }

        private CalendarCell CompileCell(DateTime start, DateTime endPeriod)
        {
            return new CalendarCell
            {
                Name = start.ToShortDateString(),
                Date = start
            };
        }

        private CalendarViewModel CompileCalendar(DateTime start, DateTime end, ReportingPeriod period, IList<CalendarCell> cells)
        {
            var periodStart = GetStartOfPeriod(start, period);
            var viewModel = new CalendarViewModel();
            while (periodStart < end)
            {
                var periodEnd = GetDateIncrementFunction(period)(periodStart);
                var row = new CalendarRow
                {
                    Cells = cells.Where(c => c.Date >= periodStart && c.Date < periodEnd).OrderBy(c => c.Date).ToList()
                };
                periodStart = periodEnd;
                viewModel.Rows.Add(row);
            }
            return viewModel;
        }

        private DateTime GetStartOfPeriod(DateTime start, ReportingPeriod period)
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
}
