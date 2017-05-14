namespace Picassi.Core.Accounts.Models.Transactions
{
    public class TransactionUploadModel
    {
        public string Description { get; set; }

        public string Date { get; set; }

        public decimal Amount { get; set; }

        public decimal Credit { get; set; }

        public decimal Debit { get; set; }

        public decimal Balance { get; set; }

        public string CategoryName { get; set; }

        public int Ordinal { get; set; }
    }
}
