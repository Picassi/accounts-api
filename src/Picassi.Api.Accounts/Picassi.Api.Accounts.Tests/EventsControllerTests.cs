using NUnit.Framework;
using Picassi.Api.Accounts.Contract.Events;
using Picassi.Api.Accounts.Tests.Framework;

namespace Picassi.Api.Accounts.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.None)]
    public class EventsControllerTests
    {        
        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        public void Test()
        {
            using (var sandbox = new SandboxWrapper())
            {
                // TODO make this test work
                sandbox.ApiClient.Events.CreateEvent(new CreateEventApiModel());
            }
        }
    }
}
