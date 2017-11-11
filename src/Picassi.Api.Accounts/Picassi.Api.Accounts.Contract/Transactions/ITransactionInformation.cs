using System;

namespace Picassi.Api.Accounts.Contract.Transactions
{
    public interface ITransactionInformation
    {
        int AccountId { get; set; }
        int? ToId { get; set; }
        decimal Amount { get; set; }
        DateTime Date { get; set; }
        string Description { get; set; }
        int? CategoryId { get; set; }
    }
}