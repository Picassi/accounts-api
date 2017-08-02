using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services.Reports;

namespace Picassi.Generator.Accounts
{
    public interface ITestDataGenerator
    {
        void GenerateTestData();
    }
    public class TestDataGenerator : ITestDataGenerator
    {
        private readonly IDictionary<Type, int> _priorities = new Dictionary<Type, int>
        {
            { typeof(Category), 1 },
            { typeof(Account), 1 },
            { typeof(Transaction), 1 }
        };

        private readonly IEnumerable<IModelDataGenerator> _generators;
        private readonly IDatabaseCleaner _cleaner;
        private readonly IAccountsDatabaseProvider _dbProvider;
        private readonly IAccountBalanceService _accountBalanceService;

        public TestDataGenerator(IEnumerable<IModelDataGenerator> generators, IDatabaseCleaner cleaner, IAccountsDatabaseProvider dbProvider, IAccountBalanceService accountBalanceService)
        {
            _generators = generators;
            _cleaner = cleaner;
            _dbProvider = dbProvider;
            _accountBalanceService = accountBalanceService;
        }

        public void GenerateTestData()
        {          
            var context = new DataGenerationContext();

            _cleaner.Clean();

            foreach (var generator in _generators.OrderBy(g => GetPriorityFor(g.Type)))
            {
                generator.Generate(context);
            }

            foreach (var account in _dbProvider.GetDataContext().Accounts)
            {
                _accountBalanceService.SetTransactionBalances(account.Id, DateTime.Today.AddDays(-90));
            }
        }

        private int GetPriorityFor(Type type)
        {
            return _priorities[type];
        }
    }
}
