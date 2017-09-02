using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Api.Accounts.Contract.Events;
using Picassi.Api.Accounts.Mappers;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.Events;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise] 
    public class EventsController : ApiController
    {
        private readonly IEventsDataService _dataService;
        private readonly IEventsApiMapper _eventsApiMapper;

        public EventsController(IEventsDataService dataService, IEventsApiMapper eventsApiMapper)
        {
            _dataService = dataService;
            _eventsApiMapper = eventsApiMapper;
        }

        [HttpGet]
        [Route("events")]
        public IEnumerable<EventModel> GetEvents([FromUri]EventsQueryJson json)
        {
            return _dataService.Query(json.PageNumber ?? 1, json.PageSize ?? 20);
        }

        [HttpPost]
        [Route("events")]
        public EventJson CreateEvent([FromBody]CreateEventJson json)
        {
            var createdEventModel = _dataService.Create(_eventsApiMapper.Map(json));

            return _eventsApiMapper.Map(createdEventModel);
        }

        [HttpGet]
        [Route("events/{id}")]
        public EventJson GetEvent(int id)
        {
            var model = _dataService.Get(id);

            return _eventsApiMapper.Map(model);
        }

        [HttpPut]
        [Route("events/{id}")]
        public EventJson UpdateEvent(int id, [FromBody]EventJson json)
        {
            var model = _dataService.Get(id);

            _eventsApiMapper.Map(json, model);

            var updatedModel = _dataService.Update(id, model);

            return _eventsApiMapper.Map(updatedModel);
        }

        [HttpDelete]
        [Route("events/{id}")]
        public bool DeleteAccount(int id)
        {
            return _dataService.Delete(id);
        }
    }
}