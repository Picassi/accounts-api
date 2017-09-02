using System.Reflection;
using System.Web.Http;
using Autofac;
using Newtonsoft.Json;
using Owin;
using Picassi.Api.Accounts.Authorization;
using Picassi.Api.Accounts.Config;
using Picassi.Api.Accounts.Filters;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Events;
using Picassi.Core.Accounts.Models.Accounts;
using Picassi.Generator.Accounts;

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
                Assembly.GetAssembly(typeof(AccountModel)),
                Assembly.GetAssembly(typeof(Account)),
                Assembly.GetAssembly(typeof(IAuthenticationConfigurator)),
                Assembly.GetAssembly(typeof(ITestDataGenerator))
            };

            var container = AutofacConfig.BuildContainer(config, webApiAssembly, 
                new AccountsApiModule(), new EventBusModule(dependentAssemblies));            

            using (var scope = container.BeginLifetimeScope())
            {
                scope.Resolve<ISwaggerConfig>().Register(config);
                scope.Resolve<IAuthenticationConfigurator>().Configure(app);
            }

            WebApiConfig.Register(config);
            JsonConfig.ConfigureJson(config, DateTimeZoneHandling.Unspecified);
            config.EnsureInitialized();
            config.Filters.Add(new ApiExceptionHandlingAttribute());

            EventBus.Instance = new DefaultEventBus(new HandlerResolver(container));

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
        }
    }
}