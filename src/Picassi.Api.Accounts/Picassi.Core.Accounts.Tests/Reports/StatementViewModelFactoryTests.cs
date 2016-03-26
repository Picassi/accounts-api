using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using NUnit.Framework;
using Picassi.Core.Accounts.Reports;
using Picassi.Core.Accounts.Tests.Dummies;
using Picassi.Core.Accounts.ViewModels.Transactions;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.Tests.Reports
{
    [TestFixture]
    public class StatementViewModelFactoryTests
    {
        private StatementViewModelFactory factory;

        [SetUp]
        public void SetUp()
        {
            factory = new StatementViewModelFactory();
        }


        [Test]
        public void FactoryCalculatesCorrectBalances()
        {
            // Arrange
            const int accountId = 1;
            const decimal startBalance = 10;
            var transactions = new List<TransactionViewModel>
            {
                A.Dummy<TransactionViewModel>().WithAmount(20).WithDate(2016, 1, 1),
                A.Dummy<TransactionViewModel>().WithAmount(25).WithDate(2016, 3, 1),
                A.Dummy<TransactionViewModel>().WithAmount(-30).WithDate(2016, 7, 1),
                A.Dummy<TransactionViewModel>().WithAmount(42).WithDate(2016, 5, 1)
            };

            // Act
            var statement = factory.CompileStatement(accountId, startBalance, transactions);

            // Assert
            Assert.AreEqual(startBalance, statement.StartBalance);
            Assert.AreEqual(67, statement.Lines.Last().Balance);
        }
    }
}
