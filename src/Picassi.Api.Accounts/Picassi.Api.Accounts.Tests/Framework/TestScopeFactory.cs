using System.Collections.Generic;
using System.Linq;
using IdentityServer3.Core.Models;

namespace Picassi.Api.Accounts.Tests.Framework
{
    public class TestScopeFactory
    {
        public static IEnumerable<Scope> GetScopes(params string[] scopes)
        {
            var customScopes = scopes.Select(BuildScope);
            return GetDefaultScopes().Union(customScopes);
        }

        private static Scope BuildScope(string scope)
        {
            return new Scope
            {
                Enabled = true,
                Name = scope,
                DisplayName = scope,
                Description = scope,
                Type = ScopeType.Resource,
                Claims = new List<ScopeClaim>()
            };
        }


        public static IEnumerable<Scope> GetDefaultScopes()
        {
            var scopes = new List<Scope>
            {
                StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.Address,
            };
            return scopes;
        }

    }
}
