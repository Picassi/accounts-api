using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Picassi.Api.Accounts.Tests.Framework
{
    public class TestJsonConfig
    {
        public static void ConfigureJson(HttpConfiguration config, DateTimeZoneHandling timeZoneHandling)
        {
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            var jSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                DateTimeZoneHandling = timeZoneHandling,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            jSettings.Converters.Add(new IsoDateTimeConverter());
            json.SerializerSettings = jSettings;

            var jsonFormatters = config.Formatters.JsonFormatter;
            jsonFormatters.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;

        }
    }
}
