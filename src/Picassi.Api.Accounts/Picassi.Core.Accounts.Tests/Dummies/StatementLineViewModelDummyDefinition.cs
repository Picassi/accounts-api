using System;
using System.Diagnostics.CodeAnalysis;
using FakeItEasy;
using Picassi.Core.Accounts.ViewModels.Statements;

namespace Picassi.Core.Accounts.Tests.Dummies
{
    [ExcludeFromCodeCoverage]
    public static class StatementLineViewModelDummyDefinitionExtensions
    {
        public static StatementLineViewModel WithAmount(this StatementLineViewModel statementLine, decimal amount)
        {
            statementLine.Amount = amount;
            statementLine.Credit = amount > 0 ? amount : 0;
            statementLine.Debit = amount < 0 ? -amount : 0;
            return statementLine;
        }

        public static StatementLineViewModel WithBalance(this StatementLineViewModel statementLine, decimal balance)
        {
            statementLine.Balance = balance;
            return statementLine;
        }

        public static StatementLineViewModel WithDate(this StatementLineViewModel statementLine, int year, int month, int day)
        {
            statementLine.Date = new DateTime(year, month, day);
            return statementLine;
        }
    }

    [ExcludeFromCodeCoverage]
    public class StatementLineViewModelDummyDefinition : DummyDefinition<StatementLineViewModel>
    {
        protected override StatementLineViewModel CreateDummy()
        {
            return new StatementLineViewModel { Description = "Dummy Transaction" };
        }
    }
}
