using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.Services;
using Picassi.Core.Accounts.ViewModels.Events;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess
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
