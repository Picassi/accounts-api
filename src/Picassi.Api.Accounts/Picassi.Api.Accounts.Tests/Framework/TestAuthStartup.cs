using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;
using Owin;
using Picassi.Auth.Config.Clients;
using Thinktecture.IdentityServer.Core.Logging;
using Thinktecture.IdentityServer.Core.Logging.LogProviders;

namespace Picassi.Api.Accounts.Tests.Framework
{
    public class TestAuthStartup
    {
        private static readonly ClientFactoryProvider ClientFactoryProvider;

        static TestAuthStartup()
        {
            var clientFactories = new List<IClientFactory>
            {
                new ApiClientFactory(),
                new MobileClientFactory(),
                new WebClientFactory(),
                new ServerClientFactory()
            };

            ClientFactoryProvider = new ClientFactoryProvider(clientFactories);
        }

        public static Action<IAppBuilder> GetAuthServerAppBuilder(TestAuthConfiguration authConfiguration)
        {
            return app =>
            {
                LogProvider.SetCurrentLogProvider(new NLogLogProvider());
                app.Map("/identity", idsrvApp =>
                {
                    var factory = new IdentityServerServiceFactory
                    {
                        CorsPolicyService =
                            new Registration<ICorsPolicyService>(new DefaultCorsPolicyService { AllowAll = true }),
                    };

                    var clients = GetClients(authConfiguration.Clients.Select(x => x.AuthConfig));
                        factory.UseInMemoryClients(clients);
                    factory.UseInMemoryUsers(authConfiguration.Users.ToList());
                    factory.UseInMemoryScopes(authConfiguration.Scopes);

                    var options = new IdentityServerOptions
                    {
                        Factory = factory,
                        SiteName = "Movebubble Security Token Service",
                        IssuerUri = authConfiguration.AuthServerUrl,
                        PublicOrigin = authConfiguration.AuthServerUrl,
                        RequireSsl = false
                    };

                    idsrvApp.UseIdentityServer(options);
                });
            };
        }

        private static IEnumerable<IdentityServer3.Core.Models.Client> GetClients(IEnumerable<IClientConfiguration> clients)
        {            
            return clients.Select(client => 
                ClientFactoryProvider.GetClientFactory(client.ClientType).GetClientDefinition(client));
        }
    }
}