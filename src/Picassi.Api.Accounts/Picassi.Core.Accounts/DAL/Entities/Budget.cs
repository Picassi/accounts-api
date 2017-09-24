using Picassi.Api.Accounts.Contract.Enums;
using Picassi.Core.Accounts.Time.Periods;

namespace Picassi.Core.Accounts.DAL.Entities
{
    public class Budget : IEntity
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public PeriodType Period { get; set; }

        public int AggregationPeriod { get; set; }

        public int CategoryId { get; set; }

        public int AccountId { get; set; }

        public virtual Category Category { get; set; }

        public virtual Account Account { get; set; }
    }
}
