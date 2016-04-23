using System;

namespace Picassi.Core.Accounts.ViewModels.Transactions
{
    public class TransactionViewModel
    {
		public int Id { get; set; }
		public string Description { get; set; }
		public int? FromId { get; set; }
		public int? ToId { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public DateTime Date { get; set; }
        public decimal In { get; set; }
        public decimal Out { get; set; }
        public string Status { get; set; }
    }
}