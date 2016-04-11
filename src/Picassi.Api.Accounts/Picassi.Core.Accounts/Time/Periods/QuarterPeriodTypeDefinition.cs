namespace Picassi.Core.Accounts.Time.Periods
{
    public class QuarterPeriodTypeDefinition : IPeriodTypeDefinition
    {
        public PeriodType Type => PeriodType.Quarter;

        public decimal GetQuantityBetweenPoints(DateRange range)
        {
            return (decimal)(range.End - range.Start).TotalDays / (Constants.DaysPerYear / (Constants.MonthsPerYear / 3));
        }
    }
}
