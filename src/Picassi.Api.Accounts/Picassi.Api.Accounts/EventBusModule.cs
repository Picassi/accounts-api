using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Picassi.Core.Accounts.Events;
using Module = Autofac.Module;

namespace Picassi.Api.Accounts
{
    public class EventBusModule : Module
    {
        private readonly Type[] _events;
        private readonly Type[] _handlers;

        public EventBusModule(params Assembly[] assemblies)
        {
            _handlers = assemblies.SelectMany(a => a.GetTypes()).Distinct()
                .Where(IsEventHandler).ToArray();
            _events = assemblies.SelectMany(a => a.GetTypes()).Distinct()
                .Where(x => typeof(IEvent).IsAssignableFrom(x) && !x.IsAbstract).ToArray();
        }

        protected override void Load(ContainerBuilder builder)
        {

            var handlersWithInterfaces = _handlers
                .SelectMany(handler => GetAllEventHandlerInterfaces(handler)
                    .Select(@interface => new { Handler = handler, Interface = @interface }))
                .ToList();


            foreach (var handlerInterface in handlersWithInterfaces)
            {
                var handledEventType = handlerInterface.Interface.GetGenericArguments().First();

                RegisterStandardHandlers(builder, handledEventType, handlerInterface.Handler);

                if (!handledEventType.IsGenericType) continue;

                RegisterGenericHandlers(builder, handledEventType, handlerInterface.Handler);
            }
        }

        private void RegisterStandardHandlers(ContainerBuilder builder, Type handledEventType, Type handlerType)
        {
            foreach (var eventType in _events.Where(e => !e.IsGenericType && handledEventType.IsAssignableFrom(e)))
            {
                builder.RegisterType(handlerType).As(typeof(IEventHandler<>).MakeGenericType(eventType));
            }
        }

        private void RegisterGenericHandlers(ContainerBuilder builder, Type handledEventType, Type handlerType)
        {
            var candidates = _events.Where(e => e.IsGenericType).Select(e => e.MakeGenericType(handledEventType.GenericTypeArguments[0]));

            foreach (var eventType in candidates.Where(handledEventType.IsAssignableFrom))
            {
                builder.RegisterType(handlerType).As(typeof(IEventHandler<>).MakeGenericType(eventType));
            }
        }

        public static bool IsEventHandler(Type target)
        {
            return GetAllEventHandlerInterfaces(target).Any();
        }

        private static IEnumerable<Type> GetAllEventHandlerInterfaces(Type target)
        {
            return GetAllInterfaces(target).Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>));
        }

        private static IEnumerable<Type> GetAllInterfaces(Type target)
        {
            foreach (var @interface in target.GetInterfaces())
            {
                yield return @interface;

                foreach (var childInterface in @interface.GetInterfaces())
                {
                    yield return childInterface;
                }
            }
        }
    }
}