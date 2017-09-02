using System.Reflection;
using Autofac;
using Picassi.Api.Accounts.Controllers;
using Picassi.Core.Accounts.DAL;
using Module = Autofac.Module;

namespace Picassi.Api.Accounts.Tests.Framework
{
    public class TestAccountsApiModule : Module
    {
        private readonly IAccountsDatabaseProvider _dbProvider;

        public TestAccountsApiModule(IAccountsDatabaseProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(
                    Assembly.GetAssembly(typeof(AccountsController)),
                    Assembly.GetAssembly(typeof(IAccountsDataContext))
                )
                .Except<AccountsDataContext>()
                .Except<AccountsDatabaseProvider>()
                .AsImplementedInterfaces();

            builder.RegisterInstance(_dbProvider).As<IAccountsDatabaseProvider>();
        }
    }
}