using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Events.Messages;

namespace Picassi.Core.Accounts.Events.Handlers
{
    public class TransactionCreatedHandler : IEventHandler<EntityCreated<Transaction>>
    {
        public void Handle(EntityCreated<Transaction> @event)
        {
            
        }
    }
}
