using Picassi.Api.Accounts.Contract.Transactions;

namespace Picassi.Core.Accounts.Services.AssignmentRules
{
    public interface IAssignmentRuleHandler
    {
        int? Priority { get; }

        bool Matches(TransactionModel model);

        void Assign(TransactionModel model);

    }
}
