using System.Collections.Generic;
using System.Linq;
using IdentityServer3.Core.Models;
using Picassi.Auth.Config.Clients;

namespace Picassi.Api.Accounts.Tests.Framework
{
    public static class TestClientFactory
    {
        public static TestApplicationConfiguration GetApiClientConfiguration(
            string clientId, 
            string baseUri,
            IEnumerable<string> allowedScopes)
        {
            var authConfiguration = new TestApplicationAuthConfiguration
            {
                ClientType = ClientType.Api,
                AccessTokenLifetime = 10800,
                AllowedScopes = allowedScopes.ToList(),
                ClientId = clientId,
                ClientName = clientId,
                RedirectUris = new List<string> { baseUri },
                PostLogoutUris = new List<string> { baseUri }
            };

            return new TestApplicationConfiguration
            {
                ClientUrl = baseUri,
                AuthConfig = authConfiguration
            };
        }

        public static TestApplicationConfiguration GetMobileClientConfiguration(string clientId, string secret, IEnumerable<string> allowedScopes)
        {
            var authConfiguration = new TestApplicationAuthConfiguration
            {
                ClientType = ClientType.Mobile,
                AccessTokenLifetime = 10800,
                AllowedScopes = allowedScopes.ToList(),
                ClientId = clientId,
                ClientName = clientId,
                Secrets = new List<Secret> { new Secret(secret.Sha256()) },
            };

            return new TestApplicationConfiguration
            {
                UnencodedSecret = secret,
                AuthConfig = authConfiguration
            };
        }

        public static TestApplicationConfiguration GetServerClientConfiguration(string clientId, string secret, 
            IEnumerable<string> allowedScopes, Dictionary<string, string> claims)
        {            
            var authConfiguration = new TestApplicationAuthConfiguration
            {
                ClientType = ClientType.Server,
                AccessTokenLifetime = 10800,
                AllowedScopes = allowedScopes.ToList(),
                ClientId = clientId,
                ClientName = clientId,
                Secrets = new List<Secret>{ new Secret(secret.Sha256()) },
                Claims = claims
            };

            return new TestApplicationConfiguration
            {
                UnencodedSecret = secret,
                AuthConfig = authConfiguration
            };
        }

    }
}
