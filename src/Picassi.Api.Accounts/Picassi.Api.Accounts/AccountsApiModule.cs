using System.Reflection;
using Autofac;
using Picassi.Core.Accounts.DAL;
using Picassi.Generator.Accounts;
using Picassi.Utils.Api.Init;
using Picassi.Utils.Data;

namespace Picassi.Api.Accounts
{
    public class AccountsApiModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IDbContext))).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(AccountsDataContext))).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ISwaggerConfig))).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ITestDataGenerator))).AsImplementedInterfaces();
        }
    }
}