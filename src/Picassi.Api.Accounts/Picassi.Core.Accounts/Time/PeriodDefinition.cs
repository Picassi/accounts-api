using Picassi.Api.Accounts.Contract.Enums;
using Picassi.Core.Accounts.Time.Periods;

namespace Picassi.Core.Accounts.Time
{
    public class PeriodDefinition
    {

        public PeriodDefinition(PeriodType period, int aggregationPeriod)
        {
            Type = period;
            Quantity = aggregationPeriod;
        }

        public decimal Quantity { get; set; }

        public PeriodType Type { get; set; }
    }
}
