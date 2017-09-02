using System;
using System.Data.Entity;
using System.Net;
using System.Reflection;
using Picassi.Api.Accounts.Client;
using Picassi.Core.Accounts.DAL;

namespace Picassi.Api.Accounts.Tests.Framework
{
    public class SandboxWrapper : IDisposable
    {
        private ApiSandbox _sandbox;

        public IPicassiAccountsApiClient ApiClient { get; }
        public IAccountsDatabaseProvider DbProvider { get; set; }

        public SandboxWrapper()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            System.Data.Entity.Database.SetInitializer(new DropCreateDatabaseAlways<AccountsDataContext>());

            DbProvider = new FakeDatabaseProvider();

            var builder = new ApiTestEnvironmentBuilder()
                .WithServicePort(TestPortProvider.GetFreeLocalhostBinding())
                .WithAuthPort(TestPortProvider.GetFreeLocalhostBinding())
                .WithWebApiAssembly(Assembly.GetAssembly(typeof(AccountsApiModule)))
                .WithAutofacModules(new TestAccountsApiModule(DbProvider))
                .WithScopes("accounts-user")
                .WithApplicationConfiguration("accounts-api", new[] { "accounts-user" })
                .WithMobileClientConfiguration("picassi-server", new[] { "accounts-user" });

            _sandbox = builder.BuildApiSandbox();

            var apiClient = builder.BuildClient("picassi-server");        
            ApiClient = new PicassiAccountsApiClient(apiClient);
        }

        public void Dispose()
        {
            _sandbox?.Dispose();
            _sandbox = null;
        }
    }
}