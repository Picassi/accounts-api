using System.Linq;
using Picassi.Api.Accounts.Contract.Transactions;

namespace Picassi.Core.Accounts.Services.AssignmentRules
{
    public interface IAssignmentRuleMatcher
    {
        IAssignmentRuleHandler FindRule(ITransactionInformation transactions);
    }

    public class AssignmentRuleMatcher : IAssignmentRuleMatcher
    {
        private readonly IAssignmentRuleProvider _assignmentRuleProvider;

        public AssignmentRuleMatcher(IAssignmentRuleProvider assignmentRuleProvider)
        {
            _assignmentRuleProvider = assignmentRuleProvider;
        }

        public IAssignmentRuleHandler FindRule(ITransactionInformation transactions)
        {
            var assignmentRules = _assignmentRuleProvider.GetAssignmentRules();

            var priorityRules = assignmentRules.OrderBy(x => x.Priority);

            return priorityRules.FirstOrDefault(x => x.Matches(transactions));
        }
    }
}
