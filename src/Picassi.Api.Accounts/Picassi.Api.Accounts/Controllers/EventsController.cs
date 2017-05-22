using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Api.Accounts.Contract.Events;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.Events;
using Picassi.Utils.Api.Attributes;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise] // TODO Ensure that we are using OWIN context for DB configuration
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
        public EventModel CreateEvent([FromBody]CreateEventApiModel model)
        {
            var eventModel = new EventModel(); // TODO fix this
            return _dataService.Create(eventModel);
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