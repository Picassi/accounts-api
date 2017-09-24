using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Picassi.Api.Accounts.Config
{
    public class JsonConfig
    {
        public static void ConfigureJson(HttpConfiguration config, DateTimeZoneHandling timeZoneHandling)
        {
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            config.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = timeZoneHandling;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.Converters = new JsonConverter[] { new StringEnumConverter(), new IsoDateTimeConverter() };

        }
    }
}
