using System;

namespace Picassi.Core.Accounts.ViewModels.Transactions
{
    public class AccountTransactionViewModel
    {
		public int Id { get; set; }
		public string Description { get; set; }
		public int AccountId { get; set; }
		public int? LinkedAccountId { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}