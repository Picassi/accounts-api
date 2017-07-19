using System.Collections.Generic;
using System.Linq;
using AutoMapper;
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
            return Mapper.Map<Transaction>(model);
        }

        public TransactionModel Map(Transaction model)
        {
            return new TransactionModel
            {
                Amount = model.Amount,
                Date = model.Date,
                AccountId = model.AccountId,
                AccountName = model.Account?.Name,
                ToId = model.AccountId,
                ToName = model.Account?.Name,
                CategoryId = model.CategoryId,
                CategoryName = model.Category?.Name,
                Description = model.Description,
                Balance = model.Balance,
                Id = model.Id,
            };
        }

        public void Patch(TransactionModel model, Transaction entity)
        {
            Mapper.Map(model, entity);
        }

        public IEnumerable<TransactionModel> MapList(IEnumerable<Transaction> results)
        {
            return results.Select(Map);
        }
    }
}