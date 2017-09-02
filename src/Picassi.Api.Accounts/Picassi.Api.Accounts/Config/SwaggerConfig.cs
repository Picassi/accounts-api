using System.Web.Http;
using Picassi.Api.Accounts.Authorization;
using Swashbuckle.Application;

namespace Picassi.Api.Accounts.Config
{
    public interface ISwaggerConfig
    {
        void Register(HttpConfiguration config);
    }

    public class SwaggerConfig : ISwaggerConfig
    {
        private readonly IAuthenticationServerSettings authSettings;

        public SwaggerConfig(IAuthenticationServerSettings authSettings)
        {
            this.authSettings = authSettings;
        }

        public void Register(HttpConfiguration config)
        {
            config.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "Modus Task Tracker");
                c.OAuth2("oauth2")
                    .Flow("implicit")
                    .AuthorizationUrl(authSettings.AuthenticationAuthority)
                    .Scopes(scopes => { scopes.Add("openid", "Open Id"); });
            })
            .EnableSwaggerUi(c =>
            {
                c.EnableOAuth2Support(authSettings.ClientId, authSettings.AuthenticationType, authSettings.ClientId);
            });
        }
    }
}
