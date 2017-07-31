using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Generator.Accounts;
using Picassi.Utils.Api.Attributes;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class TestDataController : ApiController
    {
        private readonly ITestDataGenerator _dataGenerator;

        public TestDataController(ITestDataGenerator dataGenerator)
        {
            _dataGenerator = dataGenerator;
        }

        [HttpPost]
        [Route("profile/tools/generate-test-data")]
        public void GenerateTestData()
        {
            _dataGenerator.GenerateTestData();
        }
    }
}