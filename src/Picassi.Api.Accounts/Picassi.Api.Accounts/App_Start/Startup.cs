using System.Reflection;
using System.Web.Http;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Owin;
using Picassi.Api.Accounts;
using Picassi.Api.Accounts.Filters;
using Picassi.Common.Api;
using Picassi.Common.Data;
using Picassi.Core.Accounts.DbAccess.Accounts;
using Picassi.Data.Accounts.Database;
using Swashbuckle.Application;
using Thinktecture.IdentityServer.AccessTokenValidation;

[assembly: OwinStartup(typeof(Startup))]
namespace Picassi.Api.Accounts
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureOAuth(app);

            var config = new HttpConfiguration();

            config.EnableSwagger(c => c.SingleApiVersion("v1", "Picassi Accounts API")).EnableSwaggerUi();

            StartupHelper.Configure(config, Assembly.GetExecutingAssembly(), GetAssemblyDependencies());

            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            var jSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            jSettings.Converters.Add(new IsoDateTimeConverter());
            json.SerializerSettings = jSettings;

            config.Filters.Add(new ApiExceptionHandlingAttribute());
            app.UseWebApi(config);
        }

        private Assembly[] GetAssemblyDependencies()
        {
            return new[]
            {                
                Assembly.GetAssembly(typeof (IDbContext)),
                Assembly.GetAssembly(typeof (IAccountsDataContext)),
                Assembly.GetAssembly(typeof (IAccountCrudService))
            };
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "https://picassi-auth/identity",
                RequiredScopes = new[] { "openid" }
            });            
        }
    }
}