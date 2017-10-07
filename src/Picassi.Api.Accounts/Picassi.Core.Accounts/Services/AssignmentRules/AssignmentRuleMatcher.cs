using System.Linq;
using Picassi.Api.Accounts.Contract.Transactions;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.AssignmentRules;

namespace Picassi.Core.Accounts.Services.AssignmentRules
{
    public interface IAssignmentRuleMatcher
    {
        IAssignmentRuleHandler FindRule(TransactionModel transactions);
    }

    public class AssignmentRuleMatcher : IAssignmentRuleMatcher
    {
        private readonly IAssignmentRuleProvider _assignmentRuleProvider;

        public AssignmentRuleMatcher(IAssignmentRuleProvider assignmentRuleProvider)
        {
            _assignmentRuleProvider = assignmentRuleProvider;
        }

        public IAssignmentRuleHandler FindRule(TransactionModel transactions)
        {
            var assignmentRules = _assignmentRuleProvider.GetAssignmentRules();

            var priorityRules = assignmentRules.OrderBy(x => x.Priority);

            return priorityRules.FirstOrDefault(x => x.Matches(transactions));
        }
    }
}
