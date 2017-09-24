using Picassi.Api.Accounts.Contract.Enums;

namespace Picassi.Core.Accounts.Time.Periods
{
    public interface IPeriodTypeDefinition
    {
        PeriodType Type { get; }

        decimal GetQuantityBetweenPoints(DateRange range);
    }
}
