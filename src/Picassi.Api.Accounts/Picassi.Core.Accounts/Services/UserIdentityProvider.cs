using System.Linq;
using System.Security.Claims;
using Microsoft.Owin;

namespace Picassi.Core.Accounts.Services
{
    public interface IUserIdentityProvider
    {
        string UserName { get; }

        string DisplayName { get; }
    }

    public class UserIdentityProvider : IUserIdentityProvider
    {
        private readonly IOwinContext _context;
        public const string UserNameClaimType = "username";
        public const string DisplayNameClaimType = "displayname";

        public string UserName => Principal?.Claims.Single(x => x.Type == UserNameClaimType)?.Value ?? "N/A";

        public string DisplayName => Principal?.Claims.Single(x => x.Type == DisplayNameClaimType)?.Value ?? "Anonymous";

        public ClaimsPrincipal Principal => _context.Authentication?.User;

        public UserIdentityProvider(IOwinContext context)
        {
            _context = context;
        }
    }
}
