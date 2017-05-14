using AutoMapper;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.Models.Accounts
{
    public interface IAccountModelMapper : IModelMapper<AccountModel, Account>
    {
        
    }

    public class AccountModelMapper : IAccountModelMapper
    {
        public Account CreateEntity(AccountModel model)
        {
            return Mapper.Map<Account>(model);
        }

        public AccountModel Map(Account model)
        {
            return Mapper.Map<AccountModel>(model);
        }

        public void Patch(AccountModel model, Account entity)
        {
            Mapper.Map(model, entity);
        }
    }
}