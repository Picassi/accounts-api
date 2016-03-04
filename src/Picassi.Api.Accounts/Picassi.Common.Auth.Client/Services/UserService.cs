using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Picassi.Auth.Client.Database;
using Picassi.Auth.Client.Models;

namespace Picassi.Auth.Client.Services
{
    public interface IUserService : IDisposable
    {
    }

    public class UserService : IUserService
    {
        private readonly AuthDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public UserService()
        {
            _dbContext = new AuthDbContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_dbContext));
        }

        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            var user = new IdentityUser
            {
                UserName = userModel.UserName
            };

            return await _userManager.CreateAsync(user, userModel.Password);
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            return await _userManager.FindAsync(userName, password);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            _userManager.Dispose();
        }
    }
}
