using System;
using System.Collections.Generic;
using Picassi.Api.Accounts.Contract.Transactions;
using Picassi.Core.Accounts.Services.AssignmentRules;

namespace Picassi.Core.Accounts.Services.Transactions
{
    public interface ICategoryAssignmentService
    {
        void AssignCategory(ITransactionInformation model);
        void AssignCategories(IList<TransactionUploadRecord> uploads, CreateTransactionContext context);
    }

    public class CategoryAssignmentService : ICategoryAssignmentService
    {
        private readonly IAssignmentRuleMatcher _assignmentRuleMatcher;

        public CategoryAssignmentService(IAssignmentRuleMatcher assignmentRuleMatcher)
        {
            _assignmentRuleMatcher = assignmentRuleMatcher;
        }

        public void AssignCategory(ITransactionInformation model)
        {
            var matchingRule = _assignmentRuleMatcher.FindRule(model);
            matchingRule?.Assign(model);
        }

        public void AssignCategories(IList<TransactionUploadRecord> uploads, CreateTransactionContext context)
        {
            foreach (var upload in uploads)
            {
                AssignCategory(upload.Request);
            }
        }
    }
}