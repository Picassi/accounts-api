using System.Linq;
using System.Reflection;
using FakeItEasy;
using NUnit.Framework;
using Picassi.Common.Api.Helpers;
using Picassi.Core.Accounts.DbAccess.Categories;
using Picassi.Core.Accounts.Tests.Dummies;
using Picassi.Core.Accounts.Tests.Helpers;
using Picassi.Core.Accounts.ViewModels.Categories;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.Tests.DbAccess.Categories
{
    [TestFixture]
    public class CategoryQueryServiceTests
    {
        private CategoryQueryService _queryService;
        private FakeDbSet<Category> categoryDbSet;

        [SetUp]
        public void SetUp()
        {
            AutomapperHelper.MapFromAssemblies(Assembly.GetAssembly(typeof(CategoryViewModel)));

            var dbContext = A.Fake<IAccountsDataContext>();
            categoryDbSet = new FakeDbSet<Category>();
            A.CallTo(() => dbContext.Categories).Returns(categoryDbSet);
            _queryService = new CategoryQueryService(dbContext);
        }

        [Test]
        public void EmptyQueryReturnsAllCategories()
        {
            // Arrange
            categoryDbSet.Add(A.Dummy<Category>());
            categoryDbSet.Add(A.Dummy<Category>());
            categoryDbSet.Add(A.Dummy<Category>());
            categoryDbSet.Add(A.Dummy<Category>());

            // Act
            var returnedCategories = _queryService.Query(new CategoriesQueryModel()).ToList();

            // Assert
            Assert.AreEqual(4, returnedCategories.Count);
        }

        [Test]
        public void QueryWithNameReturnsCategoriesWithNameContainingText()
        {
            // Arrange
            categoryDbSet.Add(A.Dummy<Category>().WithName("This Contains The Word Apple"));
            categoryDbSet.Add(A.Dummy<Category>().WithName("The Word Apple This Contains"));
            categoryDbSet.Add(A.Dummy<Category>().WithName("Apple is the word"));
            categoryDbSet.Add(A.Dummy<Category>().WithName("Apple"));
            categoryDbSet.Add(A.Dummy<Category>().WithName("Pear"));
            categoryDbSet.Add(A.Dummy<Category>().WithName("Orange"));
            categoryDbSet.Add(A.Dummy<Category>().WithName("Application"));

            // Act
            var returnedCategories = _queryService.Query(new CategoriesQueryModel { Name = "Apple"}).ToList();

            // Assert
            Assert.AreEqual(4, returnedCategories.Count);
        }
    }
}
