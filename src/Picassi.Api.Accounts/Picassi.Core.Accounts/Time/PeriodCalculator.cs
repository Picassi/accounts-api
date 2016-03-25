using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.Time.Periods;

namespace Picassi.Core.Accounts.Time
{
    public interface IPeriodCalculator
    {
        decimal GetNumberOfPeriods(PeriodDefinition averageSpendPeriod, DateRange range);
    }

    public class PeriodCalculator : IPeriodCalculator
    {
        private readonly IEnumerable<IPeriodTypeDefinition> _periodTypes;

        public PeriodCalculator(IEnumerable<IPeriodTypeDefinition> periodTypes)
        {
            _periodTypes = periodTypes;
        }

        public decimal GetNumberOfPeriods(PeriodDefinition averageSpendPeriod, DateRange range)
        {
            var periodType = _periodTypes.Single(x => x.Type == averageSpendPeriod.Type);
            var periodQuantity = periodType.GetQuantityBetweenPoints(range);
            return periodQuantity / averageSpendPeriod.Quantity;
        }
    }
}
