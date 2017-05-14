using AutoMapper;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.Models.Transactions
{
    public interface ITransactionModelMapper : IModelMapper<TransactionModel, Transaction>
    {
        
    }

    public class TransactionModelMapper : ITransactionModelMapper
    {
        public Transaction CreateEntity(TransactionModel model)
        {
            return Mapper.Map<Transaction>(model);
        }

        public TransactionModel Map(Transaction model)
        {
            return Mapper.Map<TransactionModel>(model);
        }

        public void Patch(TransactionModel model, Transaction entity)
        {
            Mapper.Map(model, entity);
        }
    }
}