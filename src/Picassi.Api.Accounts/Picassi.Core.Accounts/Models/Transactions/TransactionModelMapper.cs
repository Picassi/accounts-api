using System.Collections.Generic;
using System.Linq;
using Picassi.Api.Accounts.Contract.Transactions;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.Models.Transactions
{
    public interface ITransactionModelMapper : IModelMapper<TransactionModel, Transaction>
    {
        
    }

    public class TransactionModelMapper : ITransactionModelMapper
    {
        public Transaction CreateEntity(TransactionModel model)
        {
            return new Transaction
            {
                Amount = model.Amount,
                Date = model.Date,
                AccountId = model.AccountId,
                ToId = model.ToId,
                CategoryId = model.CategoryId,
                Description = model.Description,
                Balance = model.Balance,
                Id = model.Id,
                Ordinal = model.Ordinal,
                ScheduledTransactionId = model.ScheduledTransactionId
            };
        }

        public TransactionModel Map(Transaction entity)
        {
            return new TransactionModel
            {
                Amount = entity.Amount,
                Date = entity.Date,
                AccountId = entity.AccountId,
                AccountName = entity.Account?.Name,
                ToId = entity.ToId,
                ToName = entity.To?.Name,
                CategoryId = entity.CategoryId,
                CategoryName = entity.Category?.Name,
                Description = entity.Description,
                Balance = entity.Balance,
                Id = entity.Id,
                Ordinal = entity.Ordinal,
                ScheduledTransactionId = entity.ScheduledTransactionId
            };
        }

        public void Patch(TransactionModel model, Transaction entity)
        {
            entity.Amount = model.Amount;
            entity.Date = model.Date;
            entity.AccountId = model.AccountId;
            entity.ToId = model.ToId;
            entity.CategoryId = model.CategoryId;
            entity.Description = model.Description;
            entity.Balance = model.Balance;
            entity.Id = model.Id;
            entity.Ordinal = model.Ordinal;
            entity.ScheduledTransactionId = model.ScheduledTransactionId;
        }

        public IEnumerable<TransactionModel> MapList(IEnumerable<Transaction> results)
        {
            return results.Select(Map);
        }
    }
}