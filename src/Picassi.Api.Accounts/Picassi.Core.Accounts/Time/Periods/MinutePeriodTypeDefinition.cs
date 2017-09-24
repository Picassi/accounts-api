using Picassi.Api.Accounts.Contract.Enums;

namespace Picassi.Core.Accounts.Time.Periods
{
    public class MinutePeriodTypeDefinition : IPeriodTypeDefinition
    {
        public PeriodType Type => PeriodType.Minute;

        public decimal GetQuantityBetweenPoints(DateRange range)
        {
            return (decimal)(range.End - range.Start).TotalMinutes;
        }
    }
}
