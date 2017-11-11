using Picassi.Api.Accounts.Contract.Transactions;

namespace Picassi.Core.Accounts.Services.AssignmentRules
{
    public interface IAssignmentRuleHandler
    {
        int? Priority { get; }

        bool Matches(ITransactionInformation model);

        void Assign(ITransactionInformation model);

    }
}
