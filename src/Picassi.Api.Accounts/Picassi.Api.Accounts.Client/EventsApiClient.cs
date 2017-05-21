using System;
using System.Net.Http;
using Picassi.Api.Accounts.Contract.Events;
using Picassi.Auth.Clients;

namespace Picassi.Api.Accounts.Client
{
    public interface IEventsApiClient
    {

        void CreateEvent(CreateEventApiModel model);
    }

    public class EventsApiClient : IEventsApiClient
    {
        private readonly ApiClient _apiClient;

        public EventsApiClient(ApiClient client)
        {
            _apiClient = client;
        }

        public void CreateEvent(CreateEventApiModel model)
        {
            ValidateApiClient();

            var response = _apiClient.PostJson("events", model);

            if (!response.IsSuccessStatusCode) throw new HttpRequestException($"Response {response.StatusCode}: " + response.ReasonPhrase);
        }

        private void ValidateApiClient()
        {
            if (_apiClient == null) throw new InvalidOperationException("No client configured");
        }
    }
}