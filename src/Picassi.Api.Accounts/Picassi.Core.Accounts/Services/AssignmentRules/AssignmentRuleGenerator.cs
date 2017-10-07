using System;
using Picassi.Api.Accounts.Contract.Transactions;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.AssignmentRules;

namespace Picassi.Core.Accounts.Services.AssignmentRules
{
    public interface IAssignmentRuleGenerator
    {
        void GenerateRule(TransactionModel transactionModel, TransactionUpdateOptions options);
    }

    public class AssignmentRuleGenerator : IAssignmentRuleGenerator
    {
        private readonly IAssignmentRulesDataService _assignmentRulesDataService;
        private readonly IAssignmentRuleMatcher _assignmentRuleMatcher;

        public AssignmentRuleGenerator(IAssignmentRulesDataService assignmentRulesDataService, IAssignmentRuleMatcher assignmentRuleMatcher)
        {
            _assignmentRulesDataService = assignmentRulesDataService;
            _assignmentRuleMatcher = assignmentRuleMatcher;
        }

        public void GenerateRule(TransactionModel transactionModel, TransactionUpdateOptions options)
        {
            if (_assignmentRuleMatcher.FindRule(transactionModel) != null) return;

            var rule = new AssignmentRuleModel
            {
                CategoryId = transactionModel.CategoryId,
                CreatedDate = DateTime.Today,
                DescriptionRegex = transactionModel.Description,
                Enabled = true,
                Priority = null,
                Type = options.AutoRuleType
            };

            _assignmentRulesDataService.Create(rule);
        }
    }
}
