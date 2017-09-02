using System.Security.Authentication;

namespace Picassi.Api.Accounts.Client.Auth
{
    public class ClientCredentialsTokenManager : ITokenManager
    {
        private readonly IClientCredentialsProvider _clientCredentialsProvider;
        private string _clientToken;

        public ClientCredentialsTokenManager(IClientCredentialsProvider clientCredentialsProvider)
        {
            _clientCredentialsProvider = clientCredentialsProvider;
        }

        public string GetAccessToken()
        {
            _clientToken = _clientToken ?? TryGetAccessToken();

            return _clientToken;
        }

        private string TryGetAccessToken()
        {
            var tokenResponse = RequestClientToken();

            if (tokenResponse.IsError)
                throw new AuthenticationException(tokenResponse.Error);

            return tokenResponse.AccessToken;
        }


        private TokenResponse RequestClientToken()
        {
            var client = new TokenClient(_clientCredentialsProvider.TokenEndpoint, _clientCredentialsProvider.Clientid, _clientCredentialsProvider.ClientSecret);

            return client.RequestClientCredentialsAsync(_clientCredentialsProvider.RequestedScopes).Result;
        }

        public string GetAccessToken(string userName, string password)
        {
            _clientToken = _clientToken ?? TryGetAccessToken(userName, password);

            return _clientToken;
        }

        private string TryGetAccessToken(string userName, string password)
        {
            var tokenResponse = RequestClientToken(userName, password);

            if (tokenResponse.IsError)
                throw new AuthenticationException(tokenResponse.ErrorType + ": " + tokenResponse.Error);

            return tokenResponse.AccessToken;
        }


        private TokenResponse RequestClientToken(string userName, string password)
        {
            var client = new TokenClient(_clientCredentialsProvider.TokenEndpoint, _clientCredentialsProvider.Clientid, _clientCredentialsProvider.ClientSecret);

            return client.RequestResourceOwnerPasswordAsync(userName, password, _clientCredentialsProvider.RequestedScopes).Result;
        }

    }
}
