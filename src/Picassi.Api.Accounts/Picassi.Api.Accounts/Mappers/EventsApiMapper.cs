using System;
using Picassi.Api.Accounts.Contract.Events;
using Picassi.Core.Accounts.Models.Events;

namespace Picassi.Api.Accounts.Mappers
{
    public interface IEventsApiMapper
    {
        EventModel Map(CreateEventJson json);
        void Map(EventJson json, EventModel model);
        EventJson Map(EventModel model);
    }

    public class EventsApiMapper : IEventsApiMapper
    {
        public EventModel Map(CreateEventJson json)
        {
            return new EventModel
            {
                Name = json.Name,
                CategoryId = json.CategoryId,
                Date = json.Date
            };
        }

        public void Map(EventJson json, EventModel model)
        {
            json.Name = model.Name;
            json.CategoryId = model.CategoryId;
            json.Date = model.Date;
        }

        public EventJson Map(EventModel model)
        {
            return new EventJson
            {
                Id = model.Id,
                Name = model.Name,
                CategoryId = model.CategoryId,
                Date = model.Date
            };
        }
    }
}