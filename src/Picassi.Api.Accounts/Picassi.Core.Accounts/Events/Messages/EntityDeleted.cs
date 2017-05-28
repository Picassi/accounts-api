namespace Picassi.Core.Accounts.Events.Messages
{
    public class EntityDeleted<TEntity> : IEvent
    {
        private int id;

        public EntityDeleted(int id)
        {
            this.id = id;
        }
    }
}