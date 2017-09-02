using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Picassi.Api.Accounts.Contract.Events;
using Picassi.Api.Accounts.Tests.Builders;
using Picassi.Api.Accounts.Tests.Framework;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Models.Transactions;

namespace Picassi.Api.Accounts.Tests
{

    public abstract class TransactionsControllerTests
    {
        [TestFixture]
        public class GetTransactions : TransactionsControllerTests
        {
            [Test]
            public void Returns_All_Transactions()
            {
                using (var sandbox = new SandboxWrapper())
                {
                    // Arrange
                    var query = new TransactionsQueryModel();
                    var transactions = new[] { new Transaction(), new Transaction(), new Transaction() };
                    AddTransactions(sandbox, transactions);

                    // Act
                    var results = sandbox.ApiClient.Transactions.GetTransactions(query);

                    // Assert
                    results.Lines.Count().Should().Be(transactions.Length);
                }
            }

            [Test]
            public void Returns_All_Transactions_In_Date_Range()
            {
                using (var sandbox = new SandboxWrapper())
                {
                    // Arrange
                    var from = DateTime.UtcNow.AddDays(-7);
                    var to = DateTime.UtcNow.AddDays(-3);
                    var query = new TransactionsQueryModel
                    {
                        DateFrom = from,
                        DateTo = to
                    };

                    var transactions = new[]
                    {
                        new TransactionBuilder().WithDate(from.AddDays(-1)).Build(),
                        new TransactionBuilder().WithDate(from.AddDays(1)).Build(),
                        new TransactionBuilder().WithDate(to.AddDays(1)).Build(),
                    };
                    AddTransactions(sandbox, transactions);

                    // Act
                    var results = sandbox.ApiClient.Transactions.GetTransactions(query);

                    // Assert
                    results.Lines.Count().Should().Be(1);
                }

            }

            [Test]
            public void Returns_All_Transactions_In_Categories_Range()
            {
                using (var sandbox = new SandboxWrapper())
                {
                    // Arrange
                    var categoryId1 = 1;
                    var categoryId2 = 2;
                    var categoryId3 = 3;
                    var query = new TransactionsQueryModel
                    {
                        Categories = new[] {categoryId1, categoryId3}
                    };
                    var transactions = new[]
                    {
                        new TransactionBuilder().WithCategory(categoryId1).Build(),
                        new TransactionBuilder().WithCategory(categoryId2).Build(),
                        new TransactionBuilder().WithCategory(categoryId3).Build(),
                        new TransactionBuilder().WithCategory(null).Build(),
                    };
                    AddTransactions(sandbox, transactions);

                    // Act
                    var results = sandbox.ApiClient.Transactions.GetTransactions(query);

                    // Assert
                    results.Lines.Count().Should().Be(2);
                }
            }

            [Test]
            public void Returns_All_Transactions_In_Accounts_Range()
            {
                using (var sandbox = new SandboxWrapper())
                {
                    // Arrange
                    var accountId1 = 1;
                    var accountId2 = 2;
                    var accountId3 = 3;
                    var query = new TransactionsQueryModel
                    {
                        Accounts = new[] { accountId1, accountId3 }
                    };
                    var transactions = new[]
                    {
                        new TransactionBuilder().WithAccounts(accountId1).Build(),
                        new TransactionBuilder().WithAccounts(accountId2).Build(),
                        new TransactionBuilder().WithAccounts(accountId3).Build(),
                    };
                    AddTransactions(sandbox, transactions);

                    // Act
                    var results = sandbox.ApiClient.Transactions.GetTransactions(query);

                    // Assert
                    results.Lines.Count().Should().Be(2);
                }
            }

            private static void AddTransactions(SandboxWrapper sandbox, Transaction[] transactions)
            {
                var database = sandbox.DbProvider.GetDataContext();
                foreach (var transaction in transactions)
                {
                    database.Transactions.Add(transaction);
                }
            }
        }
    }
}
