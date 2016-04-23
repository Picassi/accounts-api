using AutoMapper;
using Picassi.Core.Accounts.Reports;
using Picassi.Core.Accounts.ViewModels.Transactions;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess.Transactions
{
    public interface ITransactionCrudService
    {
        TransactionViewModel CreateTransaction(TransactionViewModel transactions);
        TransactionViewModel GetTransaction(int id);
        TransactionViewModel UpdateTransaction(int id, TransactionViewModel transactions);
        bool DeleteTransaction(int id);
    }

    public class TransactionCrudService : ITransactionCrudService
    {
        private readonly IAccountsDataContext _dbContext;
        private IAccountBalanceService _accountBalanceService;

        public TransactionCrudService(IAccountsDataContext dataContext, IAccountBalanceService accountBalanceService)
        {
            _dbContext = dataContext;
            _accountBalanceService = accountBalanceService;
        }

        public TransactionViewModel CreateTransaction(TransactionViewModel transaction)
        {
            var dataModel = Mapper.Map<Transaction>(transaction);
            _dbContext.Transactions.Add(dataModel);
            _dbContext.SaveChanges();
            if (transaction.FromId != null)
                _accountBalanceService.SetTransactionBalances((int) transaction.FromId, transaction.Date);
            if (transaction.ToId != null)
                _accountBalanceService.SetTransactionBalances((int)transaction.FromId, transaction.Date);
            return Mapper.Map<TransactionViewModel>(dataModel);
        }

        public TransactionViewModel GetTransaction(int id)
        {
            var dataModel = _dbContext.Transactions.Find(id);
            return Mapper.Map<TransactionViewModel>(dataModel);
        }

        public TransactionViewModel UpdateTransaction(int id, TransactionViewModel transactions)
        {
            var dataModel = _dbContext.Transactions.Find(id);
            Mapper.Map(transactions, dataModel);
            _dbContext.SaveChanges();
            return Mapper.Map<TransactionViewModel>(dataModel);
        }

        public bool DeleteTransaction(int id)
        {
            var dataModel = _dbContext.Transactions.Find(id);
            _dbContext.Transactions.Remove(dataModel);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
