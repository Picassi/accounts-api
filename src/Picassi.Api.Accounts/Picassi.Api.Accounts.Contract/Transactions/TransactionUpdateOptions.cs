using Picassi.Api.Accounts.Contract.Enums;

namespace Picassi.Api.Accounts.Contract.Transactions
{
    public class TransactionUpdateOptions
    {
        public bool AutoGenerateRules { get; set; }
        public AssignmentRuleType AutoRuleType { get; set; }
    }
}