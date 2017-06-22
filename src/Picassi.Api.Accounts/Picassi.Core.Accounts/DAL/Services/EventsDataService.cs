using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Models.Events;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.DAL.Services
{
    public interface IEventsDataService : IGenericDataService<EventModel>
    {
        IEnumerable<EventModel> Query(int pageNumber = 1, int pageSize = 20);
    }

    public class EventsDataService : GenericDataService<EventModel, Event>, IEventsDataService
    {
        public EventsDataService(IModelMapper<EventModel, Event> modelMapper, IAccountsDatabaseProvider dbProvider) 
            : base(modelMapper, dbProvider)
        {
        }

        public IEnumerable<EventModel> Query(int pageNumber = 1, int pageSize = 20)
        {
            var skip = (pageNumber - 1) * pageSize;
            var queryResults = DbProvider.GetDataContext().Events.Skip(skip).Take(pageNumber).AsQueryable();
            return queryResults.Select(ModelMapper.Map);
        }
    }
}
