using System.Reflection;
using Autofac;
using Picassi.Api.Accounts.Config;
using Picassi.Core.Accounts.DAL;
using Picassi.Generator.Accounts;

namespace Picassi.Api.Accounts
{
    public class AccountsApiModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(AccountsDataContext))).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ISwaggerConfig))).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ITestDataGenerator))).AsImplementedInterfaces();
        }
    }
}