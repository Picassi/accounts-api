using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.InMemory;

namespace Picassi.Api.Accounts.Tests.Framework
{
    public class TestAuthConfiguration
    {
        public string AuthServerUrl { get; set; }
        public TestApplicationConfiguration[] Clients { get; set; }
        public InMemoryUser[] Users { get; set; }
        public Scope[] Scopes { get; set; }
    }
}
