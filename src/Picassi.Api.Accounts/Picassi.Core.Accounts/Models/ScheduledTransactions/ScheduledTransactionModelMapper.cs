using AutoMapper;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.Models.ScheduledTransactions
{
    public class ScheduledTransactionModelMapper : IModelMapper<ScheduledTransactionModel, ScheduledTransaction>
    {
        public ScheduledTransaction CreateEntity(ScheduledTransactionModel model)
        {
            return Mapper.Map<ScheduledTransaction>(model);
        }

        public ScheduledTransactionModel Map(ScheduledTransaction model)
        {
            return Mapper.Map<ScheduledTransactionModel>(model);
        }

        public void Patch(ScheduledTransactionModel model, ScheduledTransaction entity)
        {
            Mapper.Map(model, entity);
        }
    }
}