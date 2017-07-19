using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.Models.Events
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

        public IEnumerable<EventModel> MapList(IEnumerable<Event> results)
        {
            return results.Select(Map);
        }
    }
}