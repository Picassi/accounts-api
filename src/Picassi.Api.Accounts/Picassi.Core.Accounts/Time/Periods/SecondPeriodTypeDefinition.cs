namespace Picassi.Core.Accounts.Time.Periods
{
    public class SecondPeriodTypeDefinition : IPeriodTypeDefinition
    {
        public PeriodType Type => PeriodType.Second;

        public decimal GetQuantityBetweenPoints(DateRange range)
        {
            return (decimal) (range.Start - range.End).TotalSeconds;
        }
    }
}
