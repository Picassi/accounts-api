using Picassi.Api.Accounts.Contract.Enums;

namespace Picassi.Core.Accounts.Time.Periods
{
    public class HourPeriodTypeDefinition : IPeriodTypeDefinition
    {
        public PeriodType Type => PeriodType.Hour;

        public decimal GetQuantityBetweenPoints(DateRange range)
        {
            return (decimal)(range.End - range.Start).TotalHours;
        }
    }
}
