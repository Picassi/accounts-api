using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Core.Accounts.DAL;
using Picassi.Generator.Accounts;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class TestDataController : ApiController
    {
        private readonly ITestDataGenerator _dataGenerator;
        private readonly IAccountsDatabaseProvider _accountsDatabaseProvider;

        public TestDataController(ITestDataGenerator dataGenerator, IAccountsDatabaseProvider accountsDatabaseProvider)
        {
            _dataGenerator = dataGenerator;
            _accountsDatabaseProvider = accountsDatabaseProvider;
        }

        [HttpPost]
        [Route("profile/tools/generate-test-data")]
        public void GenerateTestData()
        {
            _dataGenerator.GenerateTestData();
        }

        [HttpPost]
        [Route("profile/tools/delete-all-transactions")]
        public void DeleteAllTransactions()
        {
            foreach (var transaction in _accountsDatabaseProvider.GetDataContext().Transactions.ToList())
            {
                _accountsDatabaseProvider.GetDataContext().Transactions.Remove(transaction);
            }
            _accountsDatabaseProvider.GetDataContext().SaveChanges();

        }

    }
}