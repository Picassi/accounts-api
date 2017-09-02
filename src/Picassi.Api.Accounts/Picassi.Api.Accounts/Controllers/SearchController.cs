using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Picassi.Api.Accounts.Contract;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.Transactions;

namespace Picassi.Api.Accounts.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [PicassiApiAuthorise]
    public class SearchController : ApiController
    {
        private readonly ITransactionsDataService _transactionsDataService;

        public SearchController(ITransactionsDataService transactionsDataService)
        {
            _transactionsDataService = transactionsDataService;
        }

        [HttpGet]
        [Route("search")]
        public IEnumerable<UniversalSearchResultSection> GetResults([FromUri] string searchText)
        {
            var transactions = _transactionsDataService.Query(searchText);

            return new List<UniversalSearchResultSection>
            {
                new UniversalSearchResultSection
                {
                    Title = "Transactions",
                    Results = transactions.Select(MapToTransactionResult)
                }
            };

        }

        private static UniversalSearchResult MapToTransactionResult(TransactionModel transaction)
        {
            return new UniversalSearchResult
            {
                ResultText = transaction.Description,
                Actions = new List<UniversalSearchResultAction>
                {
                    new UniversalSearchResultAction
                    {
                        ActionKey = transaction.Id.ToString(),
                        ActionText = "Edit",
                        ActionType = ActionType.EditTransaction
                    }
                }
            };
        }
    }
}