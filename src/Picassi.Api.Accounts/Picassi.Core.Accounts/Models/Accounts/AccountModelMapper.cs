using System;
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
            return new Account
            {
                LastUpdated = DateTime.Now,
                Name = model.Name
            };
        }

        public AccountModel Map(Account model)
        {
            return new AccountModel
            {
                Id = model.Id,
                LastUpdated = model.LastUpdated,
                Name = model.Name
            };
        }

        public void Patch(AccountModel model, Account entity)
        {
            entity.Name = model.Name;
        }
    }
}