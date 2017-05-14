using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Models.Events;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.DAL.Services
{
    public interface IEventsDataService : IGenericDataService<EventModel>
    {
        IEnumerable<EventModel> Query(EventsQueryModel query);
    }

    public class EventsDataService : GenericDataService<EventModel, Event>, IEventsDataService
    {
        public EventsDataService(IModelMapper<EventModel, Event> modelMapper, IAccountsDataContext dbContext) 
            : base(modelMapper, dbContext)
        {
        }

        public IEnumerable<EventModel> Query(EventsQueryModel query)
        {
            var queryResults = DbContext.Events.AsQueryable();

            return queryResults.Select(ModelMapper.Map);
        }

    }
}
