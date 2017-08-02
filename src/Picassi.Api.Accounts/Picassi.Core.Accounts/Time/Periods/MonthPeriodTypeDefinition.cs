using System;

namespace Picassi.Core.Accounts.Time.Periods
{
    public class MonthPeriodTypeDefinition : IPeriodTypeDefinition
    {
        public PeriodType Type => PeriodType.Month;

        public decimal GetQuantityBetweenPoints(DateRange range)
        {
            decimal monthsDifference = ((range.End.Year - range.Start.Year) * 12) + range.End.Month - range.Start.Month;
            var numDaysInMonth = DateTime.DaysInMonth(range.End.Year, range.End.Month);
            monthsDifference += (decimal) (range.End.Day - range.Start.Day) / numDaysInMonth;
            return monthsDifference;
        }
    }
}
