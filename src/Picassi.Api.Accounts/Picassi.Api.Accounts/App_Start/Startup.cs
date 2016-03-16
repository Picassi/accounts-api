using System.Net;
using System.Reflection;
using Microsoft.Owin;
using Owin;
using Picassi.Api.Accounts;
using Picassi.Common.Api;
using Picassi.Common.Api.Init;
using Picassi.Common.Data;
using Picassi.Core.Accounts.DbAccess.Accounts;
using Picassi.Data.Accounts.Database;

[assembly: OwinStartup(typeof(Startup))]
namespace Picassi.Api.Accounts
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            StartupHelper.Configure(app, Assembly.GetExecutingAssembly(), GetAssemblyDependencies());
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        private Assembly[] GetAssemblyDependencies()
        {
            return new[]
            {                
                Assembly.GetAssembly(typeof (IDbContext)),
                Assembly.GetAssembly(typeof (IAccountsDataContext)),
                Assembly.GetAssembly(typeof (IAccountCrudService)),
                Assembly.GetAssembly(typeof (ISwaggerConfig))
            };
        }
    }
}