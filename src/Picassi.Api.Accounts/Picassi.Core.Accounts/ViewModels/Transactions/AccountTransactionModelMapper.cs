using AutoMapper;
using Picassi.Core.Accounts.Services;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.ViewModels.Transactions
{
    public interface IAccountTransactionModelMapper : IModelMapper<AccountTransactionModel, Transaction>
    {
        
    }

    public class AccountTransactionModelMapper : IAccountTransactionModelMapper
    {
        public Transaction CreateEntity(AccountTransactionModel model)
        {
            return Mapper.Map<Transaction>(model);
        }

        public AccountTransactionModel Map(Transaction model)
        {
            return Mapper.Map<AccountTransactionModel>(model);
        }

        public void Patch(AccountTransactionModel model, Transaction entity)
        {
            Mapper.Map(model, entity);
        }
    }
}