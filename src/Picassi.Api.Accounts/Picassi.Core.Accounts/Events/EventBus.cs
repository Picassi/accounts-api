using Picassi.Data.Accounts.Events;

namespace Picassi.Core.Accounts.Events
{
    public interface IEventBus
    {
        void Publish(IEvent @event);
    }

    public static class EventBus
    {
        public static IEventBus Instance { get; set; }
    }
}
