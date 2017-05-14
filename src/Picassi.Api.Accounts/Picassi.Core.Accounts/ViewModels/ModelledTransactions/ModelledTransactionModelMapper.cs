using AutoMapper;
using Picassi.Core.Accounts.Services;
using Picassi.Core.Accounts.ViewModels.ModelledTransactions;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess
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