using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using Autofac;
using Picassi.Api.Accounts.Client;
using Picassi.Core.Accounts.DAL;
using Picassi.Utils.Api.Test;

namespace Picassi.Api.Accounts.Tests.Framework
{
    public class SandboxWrapper : IDisposable
    {
        private ApiSandbox _sandbox;

        public IPicassiAccountsApiClient ServerClient { get; }
        public IAccountsDataContext Database { get; set; }

        public SandboxWrapper()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            var builder = new ApiTestEnvironmentBuilder()
                .WithServicePort(TestPortProvider.GetFreeLocalhostBinding())
                .WithAuthPort(TestPortProvider.GetFreeLocalhostBinding())
                .WithWebApiAssembly(Assembly.GetAssembly(typeof(AccountsApiModule)))
                .WithAutofacModules(new AccountsApiModule())
                .WithScopes("accounts-user")
                .WithApplicationConfiguration("accounts-api", new[] { "accounts-user" })
                .WithServerClientConfiguration("movebubble-server", new[] { "accounts-user" }, new Dictionary<string, string> { { "accounts-admin", "true" } });


            _sandbox = builder.BuildApiSandbox();

            var apiClient = builder.BuildClient("movebubble-server");
            ServerClient = new PicassiAccountsApiClient(apiClient);

            //Database = (TestAccountsDataContext)TestAutofacConfig.Container.Resolve<IAccountsDataContext>();
        }

        public void Dispose()
        {
            _sandbox?.Dispose();
            _sandbox = null;
        }
    }
}