using System.Collections.Generic;
using IdentityServer3.Core.Models;
using Picassi.Auth.Config.Clients;

namespace Picassi.Api.Accounts.Tests.Framework
{
    public class TestApplicationAuthConfiguration : IClientConfiguration
    {
        public ClientType ClientType { get; set; }
        public string ClientName { get; set; }
        public string ClientId { get; set; }
        public List<string> RedirectUris { get; set; }
        public List<string> PostLogoutUris { get; set; }
        public List<string> AllowedScopes { get; set; }
        public List<Secret> Secrets { get; set; }
        public int AccessTokenLifetime { get; set; }
        public Dictionary<string, string> Claims { get; set; }
    }
}