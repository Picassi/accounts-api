using System;
using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;
using Picassi.Core.Accounts.DbAccess.Snapshots;
using Picassi.Core.Accounts.DbAccess.Transactions;
using Picassi.Core.Accounts.Reports;
using Picassi.Core.Accounts.Tests.Dummies;
using Picassi.Core.Accounts.ViewModels.Snapshots;
using Picassi.Core.Accounts.ViewModels.Transactions;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.Tests.Reports
{
    [TestFixture]
    public class AccountBalanceServiceTests
    {
        private AccountBalanceService balanceService;
        private ISnapshotQueryService snapshotQueryService;
        private ITransactionQueryService transactionQueryService;

        [SetUp]
        public void SetUp()
        {
            snapshotQueryService = A.Fake<ISnapshotQueryService>();
            transactionQueryService = A.Fake<ITransactionQueryService>();
            balanceService = new AccountBalanceService(snapshotQueryService, transactionQueryService);
        }


        [Test]
        public void ServiceReturnsBalanceAtLastTransactionIfNoInterveningSnapshot()
        {
            // Arrange
            const int accountId = 1;
            var date = new DateTime(2016, 2, 1);
            var snapshot = new SnapshotViewModel {AccountId = accountId, Date = new DateTime(2016, 1, 15), Amount = 100};
            var transactions = new List<TransactionViewModel>
            {
                A.Dummy<TransactionViewModel>().WithAmount(10).FromAccount(accountId),
                A.Dummy<TransactionViewModel>().WithAmount(20).ToAccount(accountId),
                A.Dummy<TransactionViewModel>().WithAmount(25).FromAccount(accountId),
            };

            A.CallTo(() => snapshotQueryService.GetLastSnapshotBefore(accountId, date)).Returns(snapshot);            
            A.CallTo(() => transactionQueryService.Query(A<SimpleTransactionQueryModel>.That.Matches(x =>
                x.AccountId == accountId && 
                x.DateFrom == new DateTime(2016, 1, 15) &&
                x.DateTo == new DateTime(2016, 2, 1))))
                .Returns(transactions);

            // Act
            var balance = balanceService.GetAccountBalance(accountId, date);

            // Assert
            Assert.AreEqual(85, balance);
        }

        [Test]
        public void ServiceReturnsBalanceAtLastSnapshotIfNoInterveningTransaction()
        {
            // Arrange
            const int accountId = 1;
            var date = new DateTime(2016, 2, 1);
            var snapshot = new SnapshotViewModel { AccountId = accountId, Date = new DateTime(2016, 1, 15), Amount = 100 };
            var transactions = new List<TransactionViewModel>();

            A.CallTo(() => snapshotQueryService.GetLastSnapshotBefore(accountId, date)).Returns(snapshot);
            A.CallTo(() => transactionQueryService.Query(A<SimpleTransactionQueryModel>.That.Matches(x =>
                x.AccountId == accountId &&
                x.DateFrom == new DateTime(2016, 1, 15) &&
                x.DateTo == new DateTime(2016, 2, 1))))
                .Returns(transactions);

            // Act
            var balance = balanceService.GetAccountBalance(accountId, date);

            // Assert
            Assert.AreEqual(100, balance);
        }

        [Test]
        public void ServiceReturnsZeroIfNoPrecedingTransactionOrSnapshot()
        {
            // Arrange
            const int accountId = 1;
            var date = new DateTime(2016, 2, 1);
            var transactions = new List<TransactionViewModel>();

            A.CallTo(() => snapshotQueryService.GetLastSnapshotBefore(accountId, date)).Returns(null);
            A.CallTo(() => transactionQueryService.Query(A<SimpleTransactionQueryModel>.That.Matches(x =>
                x.AccountId == accountId &&
                x.DateFrom == new DateTime(2016, 1, 15) &&
                x.DateTo == new DateTime(2016, 2, 1))))
                .Returns(transactions);

            // Act
            var balance = balanceService.GetAccountBalance(accountId, date);

            // Assert
            Assert.AreEqual(0, balance);
        }

    }
}
