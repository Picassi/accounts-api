using System;
using System.Linq;
using Picassi.Core.Accounts.Reports;
using Picassi.Core.Accounts.ViewModels.Transactions;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess.Transactions
{
    public interface IAccountTransactionCrudService
    {
        AccountTransactionViewModel CreateTransaction(int accountId, AccountTransactionViewModel transactions);
        AccountTransactionViewModel GetTransaction(int accountId, int transactionId);
        AccountTransactionViewModel UpdateTransaction(int accountId, int transactionId, AccountTransactionViewModel transaction);
        void MoveTransactionUp(int accountId, int transactionId);
        void MoveTransactionDown(int accountId, int transactionId);
        bool DeleteTransaction(int accountId, int transactionId);
    }

    public class AccountTransactionCrudService : IAccountTransactionCrudService
    {
        private readonly IAccountsDataContext _dbContext;
        private readonly IAccountBalanceService _accountBalanceService;

        public AccountTransactionCrudService(IAccountsDataContext dataContext, IAccountBalanceService accountBalanceService)
        {
            _dbContext = dataContext;
            _accountBalanceService = accountBalanceService;
        }

        public AccountTransactionViewModel CreateTransaction(int accountId, AccountTransactionViewModel transaction)
        {
            var dataModel = ConvertToDataModel(accountId, transaction);
            _dbContext.Transactions.Add(dataModel);
            _dbContext.SaveChanges();
            var precedingTransaction = _dbContext.Transactions
                .Where(t => t.FromId == accountId || t.ToId == accountId && t.Date <= transaction.Date & t.Id != dataModel.Id)
                .OrderBy(t => t.Date).ThenBy(t => t.Ordinal)
                .FirstOrDefault();
            InsertOrdinalAt(dataModel.Id, (precedingTransaction?.Ordinal ?? 0) + 1);
            _accountBalanceService.SetTransactionBalances(transaction.AccountId, transaction.Date);
            return ConvertToViewModel(accountId, dataModel);
        }

        public AccountTransactionViewModel GetTransaction(int accountId, int transactionId)
        {
            var dataModel = _dbContext.Transactions.Find(transactionId);
            return ConvertToViewModel(accountId, dataModel);
        }

        public AccountTransactionViewModel UpdateTransaction(int accountId, int transactionId, AccountTransactionViewModel transaction)
        {
            var dataModel = _dbContext.Transactions.Find(transactionId);
            var existingBalance = dataModel.Balance;
            ConvertToDataModel(accountId, transaction, dataModel);
            _dbContext.SaveChanges();

            if (dataModel.Balance != existingBalance)
            {                
                _accountBalanceService.SetTransactionBalances(transaction.AccountId, transaction.Date, dataModel, transaction.Balance - transaction.Amount);
            }

            return ConvertToViewModel(accountId, dataModel);
        }

        public void MoveTransactionUp(int accountId, int transactionId)
        {
            var transaction = _dbContext.Transactions.Find(transactionId);
            var targetTransaction = _dbContext.Transactions
                .Where(x => (x.FromId == accountId || x.ToId == accountId) && x.Date == transaction.Date && x.Ordinal > transaction.Ordinal)
                .OrderBy(x => x.Ordinal).FirstOrDefault();
            if (targetTransaction != null)
            {
                MoveOrdinalFromAToB(transactionId, transaction.Ordinal, targetTransaction.Ordinal);
            }
            else
            {
                MoveOrdinalFromAToB(transactionId, transaction.Ordinal, transaction.Ordinal + 1);
            }
            _accountBalanceService.SetTransactionBalances(accountId, transaction.Date);
        }

        public void MoveTransactionDown(int accountId, int transactionId)
        {
            var transaction = _dbContext.Transactions.Find(transactionId);
            var targetTransaction = _dbContext.Transactions
                .Where(x => (x.FromId == accountId || x.ToId == accountId) && x.Date == transaction.Date && x.Ordinal < transaction.Ordinal)
                .OrderByDescending(x => x.Ordinal).FirstOrDefault();
            if (targetTransaction != null)
            {
                MoveOrdinalFromAToB(transactionId, transaction.Ordinal, targetTransaction.Ordinal);
            }
            else
            {
                MoveOrdinalFromAToB(transactionId, transaction.Ordinal, transaction.Ordinal + 1);
            }
            _accountBalanceService.SetTransactionBalances(accountId, transaction.Date);
        }

        public bool DeleteTransaction(int accountId, int transactionId)
        {
            var dataModel = _dbContext.Transactions.Find(transactionId);
            _dbContext.Transactions.Remove(dataModel);
            _dbContext.SaveChanges();
            return true;
        }

        private static AccountTransactionViewModel ConvertToViewModel(int accountId, Transaction transaction)
        {
            return new AccountTransactionViewModel
            {
                Id = transaction.Id,
                Description = transaction.Description,
                AccountId = accountId,
                LinkedAccountId = transaction.FromId == accountId ? transaction.ToId : transaction.FromId,
                CategoryId = transaction.CategoryId,
                CategoryName = transaction.Category?.Name,
                Amount = transaction.FromId == accountId ? -transaction.Amount : transaction.Amount,
                Balance = transaction.Balance,
                Date = transaction.Date,
                Status = transaction.Status.ToString(),
            };
        }

        private static Transaction ConvertToDataModel(int accountId, AccountTransactionViewModel viewModel)
        {
            var dataModel = new Transaction();
            ConvertToDataModel(accountId, viewModel, dataModel);
            return dataModel;
        }

        private static void ConvertToDataModel(int accountId, AccountTransactionViewModel viewModel, Transaction dataModel)
        {
            dataModel.Id = viewModel.Id;
            dataModel.Description = viewModel.Description;
            dataModel.FromId = viewModel.Amount >= 0 ? viewModel.LinkedAccountId : accountId;
            dataModel.ToId = viewModel.Amount < 0 ? viewModel.LinkedAccountId : accountId;
            dataModel.CategoryId = viewModel.CategoryId;
            dataModel.Amount = viewModel.Amount < 0 ? -viewModel.Amount : viewModel.Amount;
            dataModel.Balance = viewModel.Balance;
            dataModel.Date = viewModel.Date;
            dataModel.Status = (TransactionStatus) Enum.Parse(typeof (TransactionStatus), viewModel.Status);
        }

        private void InsertOrdinalAt(int id, int position)
        {
            _dbContext.Database.ExecuteSqlCommand($"Update accounts.transactions set ordinal = ordinal + 1 where ordinal >= {position}");
            _dbContext.Database.ExecuteSqlCommand($"Update accounts.transactions set ordinal = {position} where id = {id}");
        }


        private void MoveOrdinalFromAToB(int id, int start, int end)
        {
            var update = start > end
                ? $"Update accounts.transactions set ordinal = ordinal + 1 where ordinal < {start} and ordinal >= {end}"
                : $"Update accounts.transactions set ordinal = ordinal - 1 where ordinal > {start} and ordinal <= {end}";
            _dbContext.Database.ExecuteSqlCommand(update);
            _dbContext.Database.ExecuteSqlCommand($"Update accounts.transactions set ordinal = {end} where id = {id}");
        }
    }
}
