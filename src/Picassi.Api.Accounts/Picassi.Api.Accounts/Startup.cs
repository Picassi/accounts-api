using System.Reflection;
using System.Web.Http;
using Autofac;
using Newtonsoft.Json;
using Owin;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Models.Accounts;
using Picassi.Utils.Api.Authorization;
using Picassi.Utils.Api.Filters;
using Picassi.Utils.Api.Helpers;
using Picassi.Utils.Api.Init;

namespace Picassi.Api.Accounts
{
    public class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            var webApiAssembly = Assembly.GetAssembly(typeof(Startup));
            var dependentAssemblies = new[]
            {
                webApiAssembly,
                Assembly.GetAssembly(typeof(AccountAutomapperProfile)),
                Assembly.GetAssembly(typeof(Account)),
                Assembly.GetAssembly(typeof(IAuthenticationConfigurator))
            };

            AutofacConfig.BuildContainer(config, webApiAssembly, new ApiModule());            

            using (var scope = AutofacConfig.Container.BeginLifetimeScope())
            {
                scope.Resolve<ISwaggerConfig>().Register(config);
                scope.Resolve<IAuthenticationConfigurator>().Configure(app);
            }

            WebApiConfig.Register(config);
            AutomapperHelper.MapFromAssemblies(dependentAssemblies);
            JsonConfig.ConfigureJson(config, DateTimeZoneHandling.Unspecified);
            config.EnsureInitialized();
            config.Filters.Add(new ApiExceptionHandlingAttribute());

            app.UseWebApi(config);
        }
    }
}