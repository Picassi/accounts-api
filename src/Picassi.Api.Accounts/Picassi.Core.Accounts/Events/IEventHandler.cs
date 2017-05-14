namespace Picassi.Data.Accounts.Events
{
    public interface IEventHandler<in TEvent>
    {
        void Handle(TEvent @event);
    }
}
