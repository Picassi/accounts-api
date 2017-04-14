using System.Reflection;
using Autofac;
using Picassi.Core.Accounts.DbAccess.Accounts;
using Picassi.Data.Accounts.Database;
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
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IAccountCrudService))).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ISwaggerConfig))).AsImplementedInterfaces();
        }
    }
}