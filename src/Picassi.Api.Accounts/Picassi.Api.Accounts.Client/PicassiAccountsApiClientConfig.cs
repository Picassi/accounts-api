using Newtonsoft.Json;
using Picassi.Auth.Clients;

namespace Picassi.Api.Accounts.Client
{
    public interface IPicassiAccountsApiConfig
    {
        string BaseAddress { get; }
        JsonSerializerSettings SerializerSettings { get; }
        IClientCredentialsProvider ClientCredentials { get; }
    }

    public class PicassiAccountsApiConfig : IPicassiAccountsApiConfig
    {
        public string BaseAddress { get; set; }
        public JsonSerializerSettings SerializerSettings { get; set; }
        public IClientCredentialsProvider ClientCredentials { get; set; }
    }
}