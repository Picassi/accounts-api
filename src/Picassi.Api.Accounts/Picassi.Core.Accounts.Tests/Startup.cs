using System.Reflection;
using System.Web.Http;
using Autofac;
using Newtonsoft.Json;
using Owin;
using Picassi.Core.Accounts.DAL;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Utils.Api.Authorization;
using Picassi.Utils.Api.Filters;
using Picassi.Utils.Api.Helpers;
using Picassi.Utils.Api.Init;
using Picassi.Utils.Data;
using Module = Autofac.Module;

namespace Picassi.Core.Accounts.Tests
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
                Assembly.GetAssembly(typeof(IAuthenticationConfigurator))
            };

            AutofacConfig.BuildContainer(config, webApiAssembly, new TestApiModule());            

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

    public class TestApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IDbContext))).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IAccountsDataContext))).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IAccountDataService))).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ISwaggerConfig))).AsImplementedInterfaces();
        }
    }
}