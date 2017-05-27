using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.Models.Transactions
{
    public interface IAccountTransactionModelMapper : IModelMapper<AccountTransactionModel, Transaction>
    {
        
    }

    public class AccountTransactionModelMapper : IAccountTransactionModelMapper
    {
        public Transaction CreateEntity(AccountTransactionModel model)
        {
            return new Transaction
            {
                Description = model.Description,
                CategoryId = model.CategoryId,
                AccountId = model.AccountId,
                ToId = model.LinkedAccountId,
                Date = model.Date,
                Balance = model.Balance,
                Amount = model.Amount
            };
        }

        public AccountTransactionModel Map(Transaction model)
        {
            return new AccountTransactionModel
            {
                Id = model.Id,
                Description = model.Description,
                CategoryId = model.CategoryId,
                CategoryName = model.Category?.Name,
                AccountId = model.AccountId,
                LinkedAccountId = model.ToId,
                Date = model.Date,
                Balance = model.Balance,
                Amount = model.Amount
            };
        }

        public void Patch(AccountTransactionModel model, Transaction entity)
        {
            entity.Description = model.Description;
            entity.CategoryId = model.CategoryId;
            entity.AccountId = model.AccountId;
            entity.ToId = model.LinkedAccountId;
            entity.Date = model.Date;
            entity.Balance = model.Balance;
            entity.Amount = model.Amount;
        }
    }
}