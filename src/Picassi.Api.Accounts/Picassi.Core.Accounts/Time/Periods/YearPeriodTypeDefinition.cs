namespace Picassi.Core.Accounts.Time.Periods
{
    public class YearPeriodTypeDefinition : IPeriodTypeDefinition
    {
        public PeriodType Type => PeriodType.Year;

        public decimal GetQuantityBetweenPoints(DateRange range)
        {
            return (decimal)(range.End - range.Start).TotalDays / Constants.DaysPerYear;
        }
    }
}
