using System.Collections.Generic;
using Autofac;

namespace Picassi.Core.Accounts.Events
{
    public interface IHandlerResolver
    {
        IEnumerable<IEventHandler<T>> GetHandlersFor<T>() where T : IEvent;
    }

    public class HandlerResolver : IHandlerResolver
    {
        private readonly IContainer _container;

        public HandlerResolver(IContainer container)
        {
            _container = container;
        }

        public IEnumerable<IEventHandler<T>> GetHandlersFor<T>() where T : IEvent
        {
            return _container.Resolve<IEnumerable<IEventHandler<T>>>();
        }
    }
}
