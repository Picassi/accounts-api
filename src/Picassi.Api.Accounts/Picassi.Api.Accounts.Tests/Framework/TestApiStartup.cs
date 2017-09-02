using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Reflection;
using System.Web.Http;
using Newtonsoft.Json;
using Owin;
using Picassi.Api.Accounts.Config;
using Thinktecture.IdentityServer.AccessTokenValidation;

namespace Picassi.Api.Accounts.Tests.Framework
{
    public class TestApiStartup
    {
        public static Action<IAppBuilder> GetServiceApiConfig(Assembly webApiAssembly, Autofac.Module[] modules, string authServerUrl, IEnumerable<string> scopes)
        {
            return app =>
            {
                var config = new HttpConfiguration();

                TestAutofacConfig.BuildContainer(config, webApiAssembly, modules);
                JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

                app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
                {
                    Authority = authServerUrl,
                    RequiredScopes = scopes,
                    EnableValidationResultCache = true
                });

                TestWebApiConfig.Register(config);                
                TestJsonConfig.ConfigureJson(config, DateTimeZoneHandling.Utc);
                config.EnsureInitialized();

                app.UseAutofacMiddleware(TestAutofacConfig.Container);
                app.UseAutofacWebApi(config);
                app.UseWebApi(config);
            };
        }
    }
}