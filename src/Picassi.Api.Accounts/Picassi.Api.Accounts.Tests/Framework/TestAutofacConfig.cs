using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;

namespace Picassi.Api.Accounts.Tests.Framework
{
    public class TestAutofacConfig
    {
        public static IContainer Container { get; set; }

        public static void BuildContainer(HttpConfiguration httpConfiguration, Assembly webApiAssembly, params Autofac.Module[] modules)
        {
            var builder = new ContainerBuilder();
            RegisterWebApi(httpConfiguration, webApiAssembly, builder);
            RegisterModules(builder, modules);
            Container = builder.Build();
            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(Container);
        }

        private static void RegisterWebApi(HttpConfiguration httpConfiguration, Assembly webApiAssembly, ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(webApiAssembly).AsImplementedInterfaces();
            builder.RegisterApiControllers(webApiAssembly).InstancePerRequest();
            builder.RegisterWebApiFilterProvider(httpConfiguration);
        }

        private static void RegisterModules(ContainerBuilder builder, params Autofac.Module[] modules)
        {
            foreach (var module in modules) builder.RegisterModule(module);
        }
    }
}