using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Picassi.Api.Accounts.Filters;

namespace Picassi.Api.Accounts.Config
{
    public class WebApiConfig
    {
        public const string DefaultRoute = "DefaultRoute";

        public static void Register(HttpConfiguration config)
        {
            config.EnableCors();
                        
            config.MessageHandlers.Add(new ApiRequestLogger());
            config.Services.Add(typeof(IExceptionLogger), new ApiExceptionLogger());

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(DefaultRoute, "{controller}/{id}", new { id = RouteParameter.Optional });

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.Converters = new JsonConverter[] { new StringEnumConverter()};
        }
    }
}