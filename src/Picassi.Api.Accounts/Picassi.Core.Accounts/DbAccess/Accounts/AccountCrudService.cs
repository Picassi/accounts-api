using AutoMapper;
using Picassi.Core.Accounts.ViewModels.Accounts;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess.Accounts
{
    public interface IAccountCrudService
    {
        AccountViewModel CreateAccount(AccountViewModel account);
        AccountViewModel GetAccount(int id);
        AccountViewModel UpdateAccount(int id, AccountViewModel account);
        bool DeleteAccount(int id);
    }

    public class AccountCrudService : IAccountCrudService
    {
        private readonly IAccountsDataContext _dbContext;

        public AccountCrudService(IAccountsDataContext dataContext)
        {
            _dbContext = dataContext;
        }

        public AccountViewModel CreateAccount(AccountViewModel account)
        {
            var dataModel = Mapper.Map<Account>(account);
            _dbContext.Accounts.Add(dataModel);
            _dbContext.SaveChanges();
            return Mapper.Map<AccountViewModel>(dataModel);
        }

        public AccountViewModel GetAccount(int id)
        {
            var dataModel = _dbContext.Accounts.Find(id);
            return Mapper.Map<AccountViewModel>(dataModel);
        }

        public AccountViewModel UpdateAccount(int id, AccountViewModel account)
        {
            var dataModel = _dbContext.Accounts.Find(id);
            Mapper.Map(account, dataModel);
            _dbContext.SaveChanges();
            return Mapper.Map<AccountViewModel>(dataModel);
        }

        public bool DeleteAccount(int id)
        {
            var dataModel = _dbContext.Accounts.Find(id);
            _dbContext.Accounts.Remove(dataModel);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
