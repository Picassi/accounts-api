using System.Configuration;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Picassi.Api.Accounts
{
    public class PicassiApiAuthorise : AuthorizeAttribute
    {
        private readonly bool _anonymousAuthenticationEnabled;

        public PicassiApiAuthorise()
        {
            _anonymousAuthenticationEnabled = ConfigurationManager.AppSettings["authentication-type"] == "anonymous";
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return _anonymousAuthenticationEnabled || base.IsAuthorized(actionContext);
        }
    }
}