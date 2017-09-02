using System;
using Picassi.Core.Accounts.DAL.Entities;

namespace Picassi.Api.Accounts.Tests.Builders
{
    public class TransactionBuilder
    {
        private readonly Transaction _transaction;

        public TransactionBuilder()
        {
            _transaction = new Transaction();
        }

        public TransactionBuilder WithDate(DateTime date)
        {
            _transaction.Date = date;
            return this;
        }

        public TransactionBuilder WithAccounts(int accountFrom, int? accountTo = null)
        {
            _transaction.AccountId = accountFrom;
            _transaction.ToId = accountTo;
            return this;
        }

        public TransactionBuilder WithCategory(int? categoryId)
        {
            _transaction.CategoryId = categoryId;
            return this;
        }

        public Transaction Build()
        {
            return _transaction;
        }
    }
}
