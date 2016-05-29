using System;
using AutoMapper;
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
            ConvertToDataModel(accountId, transaction, dataModel);
            _dbContext.SaveChanges();
            return ConvertToViewModel(accountId, dataModel);
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
    }
}
