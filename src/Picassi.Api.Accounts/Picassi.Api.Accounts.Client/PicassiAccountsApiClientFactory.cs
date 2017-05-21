using Picassi.Auth.Clients;

namespace Picassi.Api.Accounts.Client
{
    public class PicassiAccountsApiClientFactory
    {
        public static PicassiAccountsApiClient GetMovebubbleAgenciesApiClient(string baseApi, string tokenEndpoint, string clientId, string clientSecret, string requestedScopes)
        {
            var client = new ApiClient(baseApi, new ClientCredentialsTokenManager(new ClientCredentials
                {
                    TokenEndpoint = tokenEndpoint,
                    Clientid = clientId,
                    ClientSecret = clientSecret,
                    RequestedScopes = requestedScopes
                }));

            return new PicassiAccountsApiClient(client);
        }
    }

    public class ClientCredentials : IClientCredentialsProvider
    {
        public string TokenEndpoint { get; set; }
        public string Clientid { get; set; }
        public string ClientSecret { get; set; }
        public string RequestedScopes { get; set; }
    }
}

