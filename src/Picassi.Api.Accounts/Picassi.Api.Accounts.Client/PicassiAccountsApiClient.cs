using Picassi.Auth.Clients;

namespace Picassi.Api.Accounts.Client
{
    public interface IPicassiAccountsApiClient
    {
        IEventsApiClient Events { get; }
    }

    public class PicassiAccountsApiClient : IPicassiAccountsApiClient
    {        
        public IEventsApiClient Events { get; }

        public PicassiAccountsApiClient(ApiClient apiClient)
        {
            Events = new EventsApiClient(apiClient);
        }
    }
}
