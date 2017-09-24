using Picassi.Api.Accounts.Contract.Enums;

namespace Picassi.Core.Accounts.Time.Periods
{
    public class DayPeriodTypeDefinition : IPeriodTypeDefinition
    {
        public PeriodType Type => PeriodType.Day;

        public decimal GetQuantityBetweenPoints(DateRange range)
        {
            return (decimal)(range.End - range.Start).TotalDays;
        }
    }
}
