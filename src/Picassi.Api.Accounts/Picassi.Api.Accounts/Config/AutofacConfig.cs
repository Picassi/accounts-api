using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;

namespace Picassi.Api.Accounts.Config
{
    public class AutofacConfig
    {
        public static IContainer Container { get; set; }

        public static IContainer BuildContainer(HttpConfiguration httpConfiguration, Assembly webApiAssembly, params Autofac.Module[] modules)
        {
            var builder = new ContainerBuilder();
            RegisterWebApi(httpConfiguration, webApiAssembly, builder);

            foreach (var module in modules)
            {
                builder.RegisterModule(module);
            }

            Container = builder.Build();
            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(Container);
            return Container;
        }

        private static void RegisterWebApi(HttpConfiguration httpConfiguration, Assembly webApiAssembly, ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(webApiAssembly).AsImplementedInterfaces();
            builder.RegisterApiControllers(webApiAssembly).InstancePerRequest();
            builder.RegisterWebApiFilterProvider(httpConfiguration);
            builder.RegisterType<PicassiApiAuthorise>().PropertiesAutowired().InstancePerRequest();
        }
    }
}