using System;
using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;
using Picassi.Core.Accounts.Reports;
using Picassi.Core.Accounts.Services.Accounts;
using Picassi.Core.Accounts.Tests.Dummies;
using Picassi.Core.Accounts.ViewModels.Accounts;
using Picassi.Core.Accounts.ViewModels.Statements;

namespace Picassi.Core.Accounts.Tests.Reports
{
    [TestFixture]
    public class AccountSummaryViewModelFactoryTests
    {
        private AccountSummaryViewModelFactory factory;
        private IStatementService statementService;
        private IAccountStatusChecker statusChecker;

        [SetUp]
        public void SetUp()
        {
            statementService = A.Fake<IStatementService>();
            statusChecker = A.Fake<IAccountStatusChecker>();
            factory = new AccountSummaryViewModelFactory(statementService, statusChecker);
        }

        [Test]
        public void FactoryMapsBasicProperties()
        {
            // Arrange
            const int accountId = 1;
            var dateFrom = new DateTime(2016, 1, 1);
            var dateTo = new DateTime(2016, 4, 1);
            var period = new AccountPeriodViewModel { AccountId = accountId, From = dateFrom, To = dateTo };
            var statementLines = new List<StatementLineViewModel>();
            var statement = new StatementViewModel
            {
                Lines = statementLines
            };

            A.CallTo(() => statementService.GetStatement(accountId, A<StatementQueryModel>.That.Matches(x => x.DateFrom == dateFrom && x.DateTo == dateTo)))
                .Returns(statement);

            // Act            
            var summary = factory.GetAccountSummary(period);

            // Assert
            Assert.AreEqual(summary.AccountId, period.AccountId);
            Assert.AreEqual(summary.ComparisonDate, period.From);            
        }

        [Test]
        public void FactoryCalculatesBalance()
        {
            // Arrange
            const int accountId = 1;
            var dateFrom = new DateTime(2016, 1, 1);
            var dateTo = new DateTime(2016, 4, 1);
            var period = new AccountPeriodViewModel { AccountId = accountId, From = dateFrom, To = dateTo };
            var statementLines = new List<StatementLineViewModel>
            {
                A.Dummy<StatementLineViewModel>().WithDate(2016, 1, 1).WithAmount(10).WithBalance(100),
                A.Dummy<StatementLineViewModel>().WithDate(2016, 1, 3).WithAmount(30).WithBalance(110),
                A.Dummy<StatementLineViewModel>().WithDate(2016, 1, 6).WithAmount(-8).WithBalance(117),
                A.Dummy<StatementLineViewModel>().WithDate(2016, 1, 4).WithAmount(15).WithBalance(125),
                A.Dummy<StatementLineViewModel>().WithDate(2016, 1, 2).WithAmount(-20).WithBalance(80)
            };
            var statement = new StatementViewModel
            {
                Lines = statementLines
            };

            A.CallTo(() => statementService.GetStatement(accountId, A<StatementQueryModel>.That.Matches(x => x.DateFrom == dateFrom && x.DateTo == dateTo)))
                .Returns(statement);

            // Act            
            var summary = factory.GetAccountSummary(period);

            // Assert
            Assert.AreEqual(summary.Balance, 117);
        }

        [Test]
        public void FactoryCalculatesCreditAndDebit()
        {
            // Arrange
            const int accountId = 1;
            var dateFrom = new DateTime(2016, 1, 1);
            var dateTo = new DateTime(2016, 4, 1);
            var period = new AccountPeriodViewModel { AccountId = accountId, From = dateFrom, To = dateTo };
            var statementLines = new List<StatementLineViewModel>
            {
                A.Dummy<StatementLineViewModel>().WithDate(2016, 1, 1).WithAmount(10).WithBalance(100),
                A.Dummy<StatementLineViewModel>().WithDate(2016, 1, 3).WithAmount(30).WithBalance(110),
                A.Dummy<StatementLineViewModel>().WithDate(2016, 1, 6).WithAmount(-8).WithBalance(117),
                A.Dummy<StatementLineViewModel>().WithDate(2016, 1, 4).WithAmount(15).WithBalance(125),
                A.Dummy<StatementLineViewModel>().WithDate(2016, 1, 2).WithAmount(-20).WithBalance(80)
            };
            var statement = new StatementViewModel
            {
                Lines = statementLines
            };

            A.CallTo(() => statementService.GetStatement(accountId, A<StatementQueryModel>.That.Matches(x => x.DateFrom == dateFrom && x.DateTo == dateTo)))
                .Returns(statement);

            // Act            
            var summary = factory.GetAccountSummary(period);

            // Assert
            Assert.AreEqual(summary.TotalCredit, 55);
            Assert.AreEqual(summary.TotalDebit, 28);
            Assert.AreEqual(summary.TotalChange, 27);
        }

        [Test]
        public void FactorySetsLastUpdated()
        {
            // Arrange
            const int accountId = 1;
            var dateFrom = new DateTime(2016, 1, 1);
            var dateTo = new DateTime(2016, 4, 1);
            var period = new AccountPeriodViewModel { AccountId = accountId, From = dateFrom, To = dateTo };
            var statementLines = new List<StatementLineViewModel>
            {
                A.Dummy<StatementLineViewModel>().WithDate(2016, 1, 1).WithAmount(10).WithBalance(100),
                A.Dummy<StatementLineViewModel>().WithDate(2016, 1, 3).WithAmount(30).WithBalance(110),
                A.Dummy<StatementLineViewModel>().WithDate(2016, 1, 6).WithAmount(-8).WithBalance(117),
                A.Dummy<StatementLineViewModel>().WithDate(2016, 1, 4).WithAmount(15).WithBalance(125),
                A.Dummy<StatementLineViewModel>().WithDate(2016, 1, 2).WithAmount(-20).WithBalance(80)
            };
            var statement = new StatementViewModel
            {
                Lines = statementLines
            };

            A.CallTo(() => statementService.GetStatement(accountId, A<StatementQueryModel>.That.Matches(x => x.DateFrom == dateFrom && x.DateTo == dateTo)))
                .Returns(statement);

            A.CallTo(() => statusChecker.LastUpdated(accountId)).Returns(new DateTime(2016, 1, 10));

            // Act            
            var summary = factory.GetAccountSummary(period);

            // Assert
            Assert.AreEqual(new DateTime(2016, 1, 10), summary.LastUpdated);
        }
    }
}
