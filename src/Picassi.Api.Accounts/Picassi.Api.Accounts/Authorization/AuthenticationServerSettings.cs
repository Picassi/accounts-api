using System.Collections.Generic;
using System.Configuration;

namespace Picassi.Api.Accounts.Authorization
{
    public interface IAuthenticationServerSettings
    {
        string AuthenticationAuthority { get; }
        string ClientId { get; }
        string ClientRedirectUrl { get; }
        string AuthenticationType { get; }
        IEnumerable<string> Resources { get; set; }
    }

    public class AuthenticationServerSettings : IAuthenticationServerSettings
    {
        public string AuthenticationAuthority { get; }
        public string ClientId { get; }
        public string ClientRedirectUrl { get; }
        public string AuthenticationType { get; }
        public IEnumerable<string> Resources { get; set; }

        public AuthenticationServerSettings()
        {
            AuthenticationAuthority = ConfigurationManager.AppSettings["authentication-server-url"];
            ClientRedirectUrl = ConfigurationManager.AppSettings["authentication-redirection-url"];
            AuthenticationType = ConfigurationManager.AppSettings["authentication-type"];
            ClientId = ConfigurationManager.AppSettings["authentication-client-id"];
            Resources = new[] {ConfigurationManager.AppSettings["api-resources"]};
        }
    }
}
