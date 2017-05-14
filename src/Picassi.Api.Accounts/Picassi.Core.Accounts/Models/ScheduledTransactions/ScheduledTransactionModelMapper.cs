using AutoMapper;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.Models.ScheduledTransactions
{
    public class ScheduledTransactionModelMapper : IModelMapper<ScheduledTransactionViewModel, ScheduledTransaction>
    {
        public ScheduledTransaction CreateEntity(ScheduledTransactionViewModel model)
        {
            return Mapper.Map<ScheduledTransaction>(model);
        }

        public ScheduledTransactionViewModel Map(ScheduledTransaction model)
        {
            return Mapper.Map<ScheduledTransactionViewModel>(model);
        }

        public void Patch(ScheduledTransactionViewModel model, ScheduledTransaction entity)
        {
            Mapper.Map(model, entity);
        }
    }
}