using System.Collections.Generic;
using System.IdentityModel.Tokens;
using IdentityServer3.AccessTokenValidation;
using Owin;

namespace Picassi.Api.Accounts.Authorization
{
    public interface IAuthenticationConfigurator
    {
        void Configure(IAppBuilder app);
    }

    public class AuthenticationAuthenticationConfigurator : IAuthenticationConfigurator
    {
        private readonly IAuthenticationServerSettings _settings;

        public AuthenticationAuthenticationConfigurator(IAuthenticationServerSettings settings)
        {
            _settings = settings;
        }

        public void Configure(IAppBuilder app)
        {
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = _settings.AuthenticationAuthority,
                RequiredScopes = _settings.Resources,
                EnableValidationResultCache = true
            });
        }
    }
}