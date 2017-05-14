namespace Picassi.Core.Accounts.DAL.Entities
{
    public class Budget
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public Period Period { get; set; }

        public int AggregationPeriod { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }

    public enum Period
    {
        Week,
        Month,
        Quarter,
        Year
    }
}
