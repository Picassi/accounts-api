namespace Picassi.Core.Accounts.Time.Periods
{
    public class WeekPeriodTypeDefinition : IPeriodTypeDefinition
    {
        public PeriodType Type => PeriodType.Week;

        public decimal GetQuantityBetweenPoints(DateRange range)
        {
            return (decimal)(range.End - range.Start).TotalDays / Constants.DaysPerWeek;
        }
    }
}
