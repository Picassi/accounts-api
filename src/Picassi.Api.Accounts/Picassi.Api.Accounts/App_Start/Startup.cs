using System.Reflection;
using System.Web.Http;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Owin;
using Picassi.Api.Accounts;
using Picassi.Common.Api;
using Picassi.Common.Api.Init;
using Picassi.Common.Data;
using Picassi.Core.Accounts.DbAccess.Accounts;
using Picassi.Data.Accounts.Database;
using Swashbuckle.Application;
using Thinktecture.IdentityServer.AccessTokenValidation;

[assembly: OwinStartup(typeof(Startup))]
namespace Picassi.Api.Accounts
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            StartupHelper.Configure(app, Assembly.GetExecutingAssembly(), GetAssemblyDependencies());
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