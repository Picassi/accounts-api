using Picassi.Auth.Clients;

namespace Picassi.Api.Accounts.Client
{
    public interface IPicassiAccountsApiClient
    {
        
    }

    public class PicassiAccountsApiClient : IPicassiAccountsApiClient
    {
        private ApiClient _apiClient;

        public PicassiAccountsApiClient(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }
    }
}
