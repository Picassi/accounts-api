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
            throw new NotImplementedException();
        }

        public ModelledTransactionModel Map(ModelledTransaction model)
        {
            throw new NotImplementedException();
        }

        public void Patch(ModelledTransactionModel model, ModelledTransaction entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ModelledTransactionModel> MapList(IEnumerable<ModelledTransaction> results)
        {
            return results.Select(Map);
        }
    }
}