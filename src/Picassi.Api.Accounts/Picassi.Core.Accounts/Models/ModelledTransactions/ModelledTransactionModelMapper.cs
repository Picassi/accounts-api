using AutoMapper;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.Models.ModelledTransactions
{
    public class ModelledTransactionModelMapper : IModelMapper<ModelledTransactionModel, ModelledTransaction>
    {
        public ModelledTransaction CreateEntity(ModelledTransactionModel model)
        {
            return Mapper.Map<ModelledTransaction>(model);
        }

        public ModelledTransactionModel Map(ModelledTransaction model)
        {
            return Mapper.Map<ModelledTransactionModel>(model);
        }

        public void Patch(ModelledTransactionModel model, ModelledTransaction entity)
        {
            Mapper.Map(model, entity);
        }
    }
}