using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.Models.ModelledTransactions
{
    public class ModelledTransactionModelMapper : IModelMapper<ModelledTransactionModel, ModelledTransaction>
    {
        public ModelledTransaction CreateEntity(ModelledTransactionModel model)
        {
            return new ModelledTransaction
            {
                Amount = model.Amount,
                Date = model.Date,
                //AccountId = model.FromId,
                ToId = model.ToId,
                CategoryId = model.CategoryId,
                Description = model.Description,
                Balance = model.Balance,
                Id = model.Id,
                Ordinal = model.Ordinal,
                ScheduledTransactionId = model.ScheduledTransactionId,

            };
        }

        public ModelledTransactionModel Map(ModelledTransaction entity)
        {
            return new ModelledTransactionModel()
            {
                Amount = entity.Amount,
                Date = entity.Date,
                ToId = entity.ToId,
                CategoryId = entity.CategoryId,
                Description = entity.Description,
                Balance = entity.Balance,
                Id = entity.Id,
                Ordinal = entity.Ordinal,
                ScheduledTransactionId = entity.ScheduledTransactionId
            };

        }

        public void Patch(ModelledTransactionModel model, ModelledTransaction entity)
        {
            entity.Amount = model.Amount;
            entity.Date = model.Date;
            entity.ToId = model.ToId;
            entity.CategoryId = model.CategoryId;
            entity.Description = model.Description;
            entity.Balance = model.Balance;
            entity.Id = model.Id;
            entity.Ordinal = model.Ordinal;
            entity.ScheduledTransactionId = model.ScheduledTransactionId;
        }

        public IEnumerable<ModelledTransactionModel> MapList(IEnumerable<ModelledTransaction> results)
        {
            return results.Select(Map);
        }
    }
}