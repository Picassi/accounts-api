using System;
using System.Globalization;
using Picassi.Api.Accounts.Contract.Transactions;
using Picassi.Core.Accounts.Models.Transactions;

namespace Picassi.Core.Accounts.Services.Transactions
{
    public class TransactionUploadRecord
    {
        public TransactionUploadModel Upload { get; }
        public CreateTransactionRequest Request { get; }

        public bool? Success { get; private set; }
        public string Error { get; private set; }
        public TransactionModel Transaction { get; private set; }

        public TransactionUploadRecord(int accountId, TransactionUploadModel model)
        {
            Upload = model;
            Request = MapToRequest(accountId, model);
        }

        public void RecordSuccess(TransactionModel model)
        {
            Success = true;
            Transaction = model;
        }

        public void RecordError(string error)
        {
            Success = false;
            Error = error;
        }

        public static CreateTransactionRequest MapToRequest(int accountId, TransactionUploadModel upload)
        {
            return new CreateTransactionRequest
            {
                AccountId = accountId,
                Amount = upload.Amount ?? (upload.Credit ?? 0) - (upload.Debit ?? 0),
                Description = upload.Description,
                Date = DateTime.ParseExact(upload.Date, "dd/MM/yyyy", CultureInfo.CurrentCulture),
                Balance = upload.Balance,
                Ordinal = upload.Ordinal ?? 0
            };
        }
    }
}