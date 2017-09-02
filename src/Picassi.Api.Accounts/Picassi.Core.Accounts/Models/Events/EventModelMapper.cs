using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.Models.Events
{
    public class EventModelMapper : IModelMapper<EventModel, Event>
    {
        public Event CreateEntity(EventModel model)
        {
            return new Event
            {
                CategoryId = model.CategoryId,
                Name = model.Name,
                Date = model.Date
            };
        }

        public EventModel Map(Event model)
        {
            return new EventModel
            {
                CategoryId = model.CategoryId,
                Name = model.Name,
                Date = model.Date
            };
        }

        public void Patch(EventModel model, Event entity)
        {
            entity.CategoryId = model.CategoryId;
            entity.Name = model.Name;
            entity.Date = model.Date;
        }

        public IEnumerable<EventModel> MapList(IEnumerable<Event> results)
        {
            return results.Select(Map);
        }
    }
}