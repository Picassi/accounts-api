using System.Collections.Generic;

namespace Picassi.Api.Accounts.Contract
{
    public class UniversalSearchResultSection
    {
        public string Title { get; set; }
        public IEnumerable<UniversalSearchResult> Results { get; set; }
    }

    public class UniversalSearchResult
    {
        public string ResultText { get; set; }

        public IEnumerable<UniversalSearchResultAction> Actions { get; set; }
    }

    public class UniversalSearchResultAction
    {
        public string ActionText { get; set; }

        public ActionType ActionType { get; set; }

        public string ActionKey { get; set; }
    }

    public enum ActionType
    {
        EditTransaction
    }
}
