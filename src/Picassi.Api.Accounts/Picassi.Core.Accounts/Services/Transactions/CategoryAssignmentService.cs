using System;
using Picassi.Api.Accounts.Contract.Transactions;
using Picassi.Core.Accounts.Services.AssignmentRules;

namespace Picassi.Core.Accounts.Services.Transactions
{
    public interface ICategoryAssignmentService
    {
        void AssignCategory(TransactionModel model);
    }

    public class CategoryAssignmentService : ICategoryAssignmentService
    {
        private readonly IAssignmentRuleMatcher _assignmentRuleMatcher;

        public CategoryAssignmentService(IAssignmentRuleMatcher assignmentRuleMatcher)
        {
            _assignmentRuleMatcher = assignmentRuleMatcher;
        }

        public void AssignCategory(TransactionModel model)
        {
            var matchingRule = _assignmentRuleMatcher.FindRule(model);
            matchingRule?.Assign(model);
        }
    }
}