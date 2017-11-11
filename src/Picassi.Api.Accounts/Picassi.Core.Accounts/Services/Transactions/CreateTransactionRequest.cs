using System;
using Picassi.Api.Accounts.Contract.Transactions;

namespace Picassi.Core.Accounts.Services.Transactions
{
    public class CreateTransactionRequest : ITransactionInformation
    {
        public int AccountId { get; set; }
        public int? ToId { get; set; }
        public decimal Amount { get; set; }
        public decimal? Balance { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        public int Ordinal { get; set; }
    }
}
