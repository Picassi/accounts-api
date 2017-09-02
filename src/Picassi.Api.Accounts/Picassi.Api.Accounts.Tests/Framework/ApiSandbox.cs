using System;
using System.Collections.Generic;

namespace Picassi.Api.Accounts.Tests.Framework
{
    public class ApiSandbox : IDisposable
    {
        private IDisposable _authServer;
        private IDisposable _apiService;
        private readonly IEnumerable<int> _reservedPorts;

        public ApiSandbox(IDisposable authServer, IDisposable apiService, IEnumerable<int> reservedPorts)
        {
            _authServer = authServer;
            _apiService = apiService;
            _reservedPorts = reservedPorts;
        }

        public void Dispose()
        {
            if (_reservedPorts != null)
            {
                foreach (var port in _reservedPorts)
                {
                    TestPortProvider.ReturnLocalhostBinding(port);
                }
            }

            _authServer?.Dispose();
            _authServer = null;
            _apiService?.Dispose();
            _apiService = null;
        }
    }
}
