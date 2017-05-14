using AutoMapper;
using Picassi.Core.Accounts.Services;
using Picassi.Core.Accounts.ViewModels.Events;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess
{
    public class EventModelMapper : IModelMapper<EventModel, Event>
    {
        public Event CreateEntity(EventModel model)
        {
            return Mapper.Map<Event>(model);
        }

        public EventModel Map(Event model)
        {
            return Mapper.Map<EventModel>(model);
        }

        public void Patch(EventModel model, Event entity)
        {
            Mapper.Map(model, entity);
        }
    }
}