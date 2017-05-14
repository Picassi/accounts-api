using AutoMapper;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.Models.ModelledTransactions
{
    public class ModelledTransactionModelMapper : IModelMapper<ModelledTransactionViewModel, ModelledTransaction>
    {
        public ModelledTransaction CreateEntity(ModelledTransactionViewModel model)
        {
            return Mapper.Map<ModelledTransaction>(model);
        }

        public ModelledTransactionViewModel Map(ModelledTransaction model)
        {
            return Mapper.Map<ModelledTransactionViewModel>(model);
        }

        public void Patch(ModelledTransactionViewModel model, ModelledTransaction entity)
        {
            Mapper.Map(model, entity);
        }
    }
}