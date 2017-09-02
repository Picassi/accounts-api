using System.Security.Authentication;

namespace Picassi.Api.Accounts.Client.Auth
{
    public class ResourceOwnerTokenManager : ITokenManager
    {
        private readonly IResourceOwnerProvider _resourceOwnerProvider;
        private string _clientToken;

        public ResourceOwnerTokenManager(IResourceOwnerProvider resourceOwnerProvider)
        {
            _resourceOwnerProvider = resourceOwnerProvider;
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
                throw new AuthenticationException(tokenResponse.Error, tokenResponse.Exception);

            return tokenResponse.AccessToken;
        }


        private TokenResponse RequestClientToken()
        {
            var client = new TokenClient(_resourceOwnerProvider.TokenEndpoint, _resourceOwnerProvider.Clientid, _resourceOwnerProvider.ClientSecret);

            return client.RequestResourceOwnerPasswordAsync(_resourceOwnerProvider.Username, _resourceOwnerProvider.Password, _resourceOwnerProvider.RequestedScopes).Result;
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
            var client = new TokenClient(_resourceOwnerProvider.TokenEndpoint, _resourceOwnerProvider.Clientid, _resourceOwnerProvider.ClientSecret);

            return client.RequestResourceOwnerPasswordAsync(userName, password, _resourceOwnerProvider.RequestedScopes).Result;
        }

    }
}
