using System.Web.Http;

namespace Picassi.Api.Accounts.Tests.Framework
{
    public class TestWebApiConfig
    {
        public const string DefaultRoute = "DefaultRoute";

        public static void Register(HttpConfiguration config)
        {
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.MapHttpAttributeRoutes();
        }
    }
}