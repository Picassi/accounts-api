namespace Picassi.Core.Accounts.Time.Periods
{
    public interface IPeriodTypeDefinition
    {
        PeriodType Type { get; }

        decimal GetQuantityBetweenPoints(DateRange range);
    }

    public enum PeriodType
    {
        Second,
        Minute,
        Hour,
        Day,
        Week,
        Month,
        Quarter,
        Year
    }
}
