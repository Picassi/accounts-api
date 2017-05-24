using Picassi.Api.Accounts.Contract.Events;
using Picassi.Auth.Clients;

namespace Picassi.Api.Accounts.Client
{
    public interface IEventsApiClient
    {

        void CreateEvent(CreateEventJson model);
    }

    public class EventsApiClient : AbstractApiClient, IEventsApiClient
    {
        public void QueryEvents(EventsQueryJson json)
        {
            ValidateApiClient();

            var response = ApiClient.PostJson("events", json);

            HandleErrorResponse(response);
        }

        public void CreateEvent(CreateEventJson json)
        {
            ValidateApiClient();

            var response = ApiClient.PostJson("events", json);

            HandleErrorResponse(response);
        }

        public void GetEvent(string id, EventJson json)
        {
            ValidateApiClient();

            var response = ApiClient.PostJson($"events/{id}", json);

            HandleErrorResponse(response);
        }

        public void UpdateEvent(string id, EventJson json)
        {
            ValidateApiClient();

            var response = ApiClient.PutJson($"events/{id}", json);

            HandleErrorResponse(response);
        }

        public void DeleteEvent(string id)
        {
            ValidateApiClient();

            var response = ApiClient.Delete($"events/{id}");

            HandleErrorResponse(response);
        }

        public EventsApiClient(ApiClient apiClient) : base(apiClient)
        {
        }
    }
}