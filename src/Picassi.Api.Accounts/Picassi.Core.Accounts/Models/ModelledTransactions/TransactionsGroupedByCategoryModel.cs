namespace Picassi.Core.Accounts.Models.ModelledTransactions
{
    public class TransactionsGroupedByCategoryModel
    {            
        public string Description { get; set; }

        public int? CategoryId { get; set; }

        public decimal Credit { get; set; }

        public decimal Debit { get; set; }

        public decimal Amount => Credit + Debit;

        public int Count { get; set; }
    }
}