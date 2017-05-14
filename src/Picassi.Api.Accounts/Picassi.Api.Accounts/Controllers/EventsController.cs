using System.Collections.Generic;
using System.Web.Http;
using Picassi.Core.Accounts.DbAccess;
using Picassi.Core.Accounts.ViewModels.Events;

namespace Picassi.Api.Accounts.Controllers
{
    public class EventsController : ApiController
    {
        private readonly IEventsDataService _dataService;

        public EventsController(IEventsDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        [Route("events")]
        public IEnumerable<EventModel> GetEvents([FromUri]EventsQueryModel query)
        {
            return _dataService.Query(query);
        }

        [HttpPost]
        [Route("events")]
        public EventModel CreateEvent([FromBody]EventModel model)
        {
            return _dataService.Create(model);
        }

        [HttpGet]
        [Route("events/{id}")]
        public EventModel GetEvent(int id)
        {
            return _dataService.Get(id);
        }

        [HttpPut]
        [Route("events/{id}")]
        public EventModel UpdateEvent(int id, [FromBody]EventModel model)
        {
            return _dataService.Update(id, model);
        }

        [HttpDelete]
        [Route("events/{id}")]
        public bool DeleteAccount(int id)
        {
            return _dataService.Delete(id);
        }
    }
}