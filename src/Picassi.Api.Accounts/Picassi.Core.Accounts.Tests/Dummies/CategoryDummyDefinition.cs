using System.Diagnostics.CodeAnalysis;
using FakeItEasy;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.Tests.Dummies
{
    [ExcludeFromCodeCoverage]
    public static class CategoryDummyDefinitionExtensions
    {
        public static Category WithId(this Category category, int id)
        {
            category.Id = id;
            return category;
        }

        public static Category WithName(this Category category, string name)
        {
            category.Name = name;
            return category;
        }
    }

    [ExcludeFromCodeCoverage]
    public class CategoryDummyDefinition : DummyDefinition<Category>
    {
        private static int _id;

        protected override Category CreateDummy()
        {
            return new Category { Id = _id++, Name = "Dummy Category" };
        }
    }
}
