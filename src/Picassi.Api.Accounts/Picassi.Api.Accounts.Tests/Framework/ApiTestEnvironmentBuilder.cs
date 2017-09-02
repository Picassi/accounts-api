using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IdentityServer3.Core.Services.InMemory;
using Microsoft.Owin.Hosting;
using Picassi.Api.Accounts.Client.Auth;
using Module = Autofac.Module;

namespace Picassi.Api.Accounts.Tests.Framework
{
    public class ApiTestEnvironmentBuilder
    {
        private Assembly _webApiAssembly;
        private Module[] _modules;

        private TestApplicationConfiguration _testApplicationConfiguration;
        private readonly Dictionary<string, TestApplicationConfiguration> _clientConfigurations = new Dictionary<string, TestApplicationConfiguration>();
        private readonly Dictionary<string, TestUserConfig> _userConfigurations = new Dictionary<string, TestUserConfig>();
        private int _servicePort;
        private int _authPort;
        private string[] _scopes;

        public ApiTestEnvironmentBuilder WithWebApiAssembly(Assembly assembly)
        {
            _webApiAssembly = assembly;
            return this;
        }

        public ApiTestEnvironmentBuilder WithAutofacModules(params Module[] module)
        {
            _modules = module;
            return this;
        }

        public ApiTestEnvironmentBuilder WithApplicationConfiguration(string name, IEnumerable<string> clientScopes)
        {
            var configuration = TestClientFactory.GetApiClientConfiguration(name, $"http://localhost:{_servicePort}/", clientScopes);

            _testApplicationConfiguration = configuration;
            return this;
        }

        public ApiTestEnvironmentBuilder WithServerClientConfiguration(string name, IEnumerable<string> clientScopes, Dictionary<string, string> claims)
        {
            var configuration = TestClientFactory.GetServerClientConfiguration(name, TestSecretGenerator.GetSecret(), clientScopes, claims);

            _clientConfigurations[name] = configuration;
            _userConfigurations[name] = new TestUserConfig
            {
                Username = TestSecretGenerator.GetName(),
                Password = TestSecretGenerator.GetPassword()
            };
            return this;
        }

        public ApiTestEnvironmentBuilder WithApiClientConfiguration(string name, IEnumerable<string> clientScopes)
        {
            var configuration = TestClientFactory.GetApiClientConfiguration(name, $"http://localhost:{_servicePort}/", clientScopes);

            _clientConfigurations[name] = configuration;
            _userConfigurations[name] = new TestUserConfig
            {
                Username = TestSecretGenerator.GetName(),
                Password = TestSecretGenerator.GetPassword()
            };
            return this;
        }

        public ApiTestEnvironmentBuilder WithMobileClientConfiguration(string name, IEnumerable<string> clientScopes)
        {
            var configuration = TestClientFactory.GetMobileClientConfiguration(name, $"http://localhost:{_servicePort}/", clientScopes);

            _clientConfigurations[name] = configuration;
            _userConfigurations[name] = new TestUserConfig
            {
                Username = TestSecretGenerator.GetName(),
                Password = TestSecretGenerator.GetPassword()
            };
            return this;
        }

        public ApiTestEnvironmentBuilder WithScopes(params string[] scopes)
        {
            _scopes = scopes;
            return this;
        }

        public ApiTestEnvironmentBuilder WithAuthPort(int port)
        {
            _authPort = port;
            return this;
        }

        public ApiTestEnvironmentBuilder WithServicePort(int port)
        {
            _servicePort = port;
            return this;
        }

        public ApiSandbox BuildApiSandbox()
        {
            return new ApiSandbox(BuildAuthServer(), BuildTestServer(), new List<int> { _authPort, _servicePort });
        }

        public ApiClient BuildClient(string name)
        {
            var client = _clientConfigurations[name];
            var user = _userConfigurations[name];
            var authConfig = BuildAuthConfiguration();
            return new ApiClient(_testApplicationConfiguration.ClientUrl, TokenManagerProvider.GetTokenManager(authConfig.AuthServerUrl, client, user));
        }

        private IDisposable BuildAuthServer()
        {
            var authConfig = BuildAuthConfiguration();
            return WebApp.Start(authConfig.AuthServerUrl, TestAuthStartup.GetAuthServerAppBuilder(authConfig));
        }

        private IDisposable BuildTestServer()
        {
            var authConfig = BuildAuthConfiguration();
            return WebApp.Start(_testApplicationConfiguration.ClientUrl, TestApiStartup.GetServiceApiConfig(
                _webApiAssembly, _modules, authConfig.AuthServerUrl + "identity/", new string[0]));
        }

        private TestAuthConfiguration BuildAuthConfiguration()
        {
            return new TestAuthConfiguration
            {
                AuthServerUrl = $"https://localhost:{_authPort}/",
                Clients = _clientConfigurations.Values.ToArray(),
                Scopes = TestScopeFactory.GetScopes(_scopes).ToArray(),
                Users = _userConfigurations.Values.Select(u => new InMemoryUser
                {
                    Username = u.Username,
                    Password = u.Password,
                    Enabled = true,
                    Subject = u.Username
                }).ToArray()
            };
        }
    }
}
