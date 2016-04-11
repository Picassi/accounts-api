namespace Picassi.Core.Accounts.Time.Periods
{
    public class MonthPeriodTypeDefinition : IPeriodTypeDefinition
    {
        public PeriodType Type => PeriodType.Month;

        public decimal GetQuantityBetweenPoints(DateRange range)
        {
            return (decimal)(range.End - range.Start).TotalDays / (Constants.DaysPerYear / Constants.MonthsPerYear);
        }
    }
}
