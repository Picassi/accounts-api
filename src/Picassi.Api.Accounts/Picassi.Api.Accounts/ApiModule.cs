using System.Reflection;
using Autofac;
using Picassi.Core.Accounts.DAL;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Utils.Api.Init;
using Picassi.Utils.Data;

namespace Picassi.Api.Accounts
{
    public class ApiModule : Autofac.Module
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