using System.Configuration;

namespace Picassi.Auth.Client
{
    public interface IAuthSettings
    {
        string AuthenticationAuthority { get; }
        string ClientId { get; }
        string ClientRedirectUrl { get; }
        string AuthenticationType { get; }
    }

    public class AuthSettings : IAuthSettings
    {
        public string AuthenticationAuthority { get; }
        public string ClientId { get; }
        public string ClientRedirectUrl { get; }
        public string AuthenticationType { get; }

        public AuthSettings()
        {
            AuthenticationAuthority = ConfigurationManager.AppSettings["AuthServerUrl"];
            ClientRedirectUrl = ConfigurationManager.AppSettings["RedirectUrl"];
            AuthenticationType = ConfigurationManager.AppSettings["AuthenticationType"];
            ClientId = ConfigurationManager.AppSettings["ClientId"];
        }
    }
}
