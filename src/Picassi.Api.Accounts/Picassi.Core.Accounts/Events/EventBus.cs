namespace Picassi.Core.Accounts.Events
{
    public interface IEventBus
    {
        void Publish<TMessage>(TMessage @event) where TMessage : class, IEvent;
    }

    public static class EventBus
    {
        public static IEventBus Instance { get; set; }
    }
}

