using System;

namespace Picassi.Api.Accounts.Contract.Transactions
{
    public class TransactionModel
    {
		public int Id { get; set; }
        public int Ordinal { get; set; }
		public string Description { get; set; }
		public int AccountId { get; set; }
        public string AccountName { get; set; }
        public int? ToId { get; set; }
        public string ToName { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public DateTime Date { get; set; }
    }
}