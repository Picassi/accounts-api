using System;
using System.Diagnostics.CodeAnalysis;
using FakeItEasy;
using Picassi.Core.Accounts.ViewModels.Transactions;

namespace Picassi.Core.Accounts.Tests.Dummies
{
    [ExcludeFromCodeCoverage]
    public static class TransactionViewModelDummyDefinitionExtensions
    {
        public static TransactionViewModel WithId(this TransactionViewModel transaction, int id)
        {
            transaction.Id = id;
            return transaction;
        }

        public static TransactionViewModel WithName(this TransactionViewModel transaction, string description)
        {
            transaction.Description = description;
            return transaction;
        }

        public static TransactionViewModel WithAmount(this TransactionViewModel transaction, decimal amount)
        {
            transaction.Amount = amount;
            return transaction;
        }

        public static TransactionViewModel WithDate(this TransactionViewModel transaction, int year, int month, int day)
        {
            transaction.Date = new DateTime(year, month, day);
            return transaction;
        }

        public static TransactionViewModel FromAccount(this TransactionViewModel transaction, int accountId)
        {
            transaction.FromId = accountId;
            return transaction;
        }

        public static TransactionViewModel ToAccount(this TransactionViewModel transaction, int accountId)
        {
            transaction.ToId = accountId;
            return transaction;
        }
    }

    [ExcludeFromCodeCoverage]
    public class TransactionViewModelDummyDefinition : DummyDefinition<TransactionViewModel>
    {
        private static int _id;

        protected override TransactionViewModel CreateDummy()
        {
            return new TransactionViewModel { Id = _id++, Description = "Dummy Transaction" };
        }
    }
}
