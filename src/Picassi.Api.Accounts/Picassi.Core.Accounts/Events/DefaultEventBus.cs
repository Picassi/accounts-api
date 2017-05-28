using System.Linq;

namespace Picassi.Core.Accounts.Events
{
        public class DefaultEventBus : IEventBus
        {
            private readonly IHandlerResolver _resolver;

            public DefaultEventBus(IHandlerResolver resolver)
            {
                _resolver = resolver;
            }

            public void Publish<TMessage>(TMessage @event) where TMessage : class, IEvent
            {
                if (@event == null)
                {
                    return;
                }

                var resolvers = _resolver.GetHandlersFor<TMessage>().ToList();

                foreach (var handler in resolvers)
                {
                    handler.Handle(@event);
                }
            }
        }
    }
