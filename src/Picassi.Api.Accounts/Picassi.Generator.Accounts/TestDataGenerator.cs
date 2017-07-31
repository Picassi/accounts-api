using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;

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

        public TestDataGenerator(IEnumerable<IModelDataGenerator> generators, IDatabaseCleaner cleaner)
        {
            _generators = generators;
            _cleaner = cleaner;
        }

        public void GenerateTestData()
        {          
            var context = new DataGenerationContext();

            _cleaner.Clean();

            foreach (var generator in _generators.OrderBy(g => GetPriorityFor(g.Type)))
            {
                generator.Generate(context);
            }
        }

        private int GetPriorityFor(Type type)
        {
            return _priorities[type];
        }
    }
}
