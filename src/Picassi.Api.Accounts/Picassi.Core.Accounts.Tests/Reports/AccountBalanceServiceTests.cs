using System;
using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;
using Picassi.Core.Accounts.DbAccess.Transactions;
using Picassi.Core.Accounts.Reports;
using Picassi.Core.Accounts.Tests.Dummies;
using Picassi.Core.Accounts.ViewModels.Transactions;
using Picassi.Data.Accounts.Database;

namespace Picassi.Core.Accounts.Tests.Reports
{
    [TestFixture]
    public class AccountBalanceServiceTests
    {

        [SetUp]
        public void SetUp()
        {
        }


        [Test]
        public void ServiceReturnsBalanceAtLastTransactionIfNoInterveningSnapshot()
        {
        }

        [Test]
        public void ServiceReturnsBalanceAtLastSnapshotIfNoInterveningTransaction()
        {
        }

        [Test]
        public void ServiceReturnsZeroIfNoPrecedingTransactionOrSnapshot()
        {
        }
    }
}
