using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Picassi.Api.Accounts.Tests.Framework
{
    public class TestPortProvider
    {
        private const int MinimumPort = 44300;
        private const int MaximumPort = 44499;

        private static readonly object PortLock = new object();
        private static readonly Dictionary<int, bool> AvailablePorts = new Dictionary<int, bool>();

        static TestPortProvider()
        {
            for (var port = MinimumPort; port <= MaximumPort; port++)
            {
                AvailablePorts.Add(port, false);
            }
        }

        public static int GetFreeLocalhostBinding()
        {
            int port;
            lock (PortLock)
            {
                var tries = 0;

                while (true)
                {
                    if (AvailablePorts.Any(x => !x.Value))
                    {
                        port = AvailablePorts.First(x => !x.Value).Key;
                        AvailablePorts[(int)port] = true;
                        break;
                    }

                    Thread.Sleep(1);
                    tries++;

                    if (tries > 5)
                    {
                        throw new Exception("No free ports after max tries");
                    };
                }
            }

            return port;
        }

        public static void ReturnLocalhostBinding(int port)
        {
            lock (PortLock)
            {
                AvailablePorts[port] = false;
            }
        }
    }
}
