using AutoMapper;
using Picassi.Core.Accounts.Services;
using Picassi.Core.Accounts.ViewModels.ScheduledTransactions;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess
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