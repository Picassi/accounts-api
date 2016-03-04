using Microsoft.AspNet.Identity.EntityFramework;

namespace Picassi.Auth.Client.Database
{
    public class AuthDbContext : IdentityDbContext<IdentityUser>
    {
        public AuthDbContext() : base("AuthDbContext")
        {

        }
    }
}