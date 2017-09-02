using System;
using Picassi.Api.Accounts.Client.Auth;
using Picassi.Auth.Config.Clients;

namespace Picassi.Api.Accounts.Tests.Framework
{
    public static class TokenManagerProvider
    {
        public static ITokenManager GetTokenManager(string authServerUrl, TestApplicationConfiguration appConfig, TestUserConfig userConfig = null)
        {
            switch (appConfig.AuthConfig.ClientType)
            {
                case ClientType.Mobile:
                    return new ResourceOwnerTokenManager(GetResourceOwner(authServerUrl, appConfig, userConfig));
                case ClientType.Api:
                    throw new NotImplementedException("No token manager configured for api client");
                case ClientType.Server:
                    return new ClientCredentialsTokenManager(GetClientCredentials(authServerUrl, appConfig));
                case ClientType.Web:
                    throw new NotImplementedException("No token manager configured for web client");
                default:
                    throw new InvalidOperationException("Unrecognised client type");
            }
        }

        private static IClientCredentialsProvider GetClientCredentials(string authServerUrl, TestApplicationConfiguration appConfig)
        {
            return new ClientCredentialsProvider
            {
                TokenEndpoint = new Uri(new Uri(authServerUrl), "identity/connect/token").ToString(),
                Clientid = appConfig.AuthConfig.ClientId,
                ClientSecret = appConfig.UnencodedSecret,
                RequestedScopes = string.Join(" ", appConfig.AuthConfig.AllowedScopes)
            };
        }

        private static IResourceOwnerProvider GetResourceOwner(string authServerUrl, TestApplicationConfiguration appConfig, TestUserConfig userConfig)
        {
            return new ResourceOwnerProvider
            {
                TokenEndpoint = new Uri(new Uri(authServerUrl), "identity/connect/token").ToString(),
                Clientid = appConfig.AuthConfig.ClientId,
                ClientSecret = appConfig.UnencodedSecret,
                RequestedScopes = string.Join(" ", appConfig.AuthConfig.AllowedScopes),
                Username = userConfig.Username,
                Password = userConfig.Password
            };
        }
    }

    internal class ClientCredentialsProvider : IClientCredentialsProvider
    {
        public string TokenEndpoint { get; set; }
        public string Clientid { get; set; }
        public string ClientSecret { get; set; }
        public string RequestedScopes { get; set; }
    }

    internal class ResourceOwnerProvider : IResourceOwnerProvider
    {
        public string TokenEndpoint { get; set; }
        public string Clientid { get; set; }
        public string ClientSecret { get; set; }
        public string RequestedScopes { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
