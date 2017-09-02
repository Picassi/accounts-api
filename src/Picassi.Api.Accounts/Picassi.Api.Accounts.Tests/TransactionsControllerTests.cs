using Picassi.Api.Accounts.Contract.Events;
using Picassi.Api.Accounts.Tests.Framework;

namespace Picassi.Api.Accounts.Tests
{

    public abstract class TransactionsControllerTests
    {
        public class GetTransactions : TransactionsControllerTests
        {
            public void Returns_All_Transactions()
            {
                using (var sandbox = new SandboxWrapper())
                {
                    // TODO make this test work
                    sandbox.ApiClient.Events.CreateEvent(new CreateEventJson());
                }
            }

            public void Returns_All_Transactions_In_Date_Range()
            {
                
            }

            public void Returns_All_Transactions_In_Categories_Range()
            {
                
            }

            public void Returns_All_Transactions_In_Accounts_Range()
            {
                
            }
        }
    }
}
