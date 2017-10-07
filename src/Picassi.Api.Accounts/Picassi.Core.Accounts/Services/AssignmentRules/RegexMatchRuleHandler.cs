using System;
using System.Text.RegularExpressions;
using Picassi.Api.Accounts.Contract.Transactions;
using Picassi.Core.Accounts.Models.AssignmentRules;

namespace Picassi.Core.Accounts.Services.AssignmentRules
{
    public class RegexMatchRuleHandler : IAssignmentRuleHandler
    {
        public int? Priority { get; }

        private readonly Regex _text;
        private readonly int? _categoryId;

        public RegexMatchRuleHandler(AssignmentRuleModel assignmentRuleModel)
        {
            Priority = assignmentRuleModel.Priority;
            _text = new Regex(assignmentRuleModel.DescriptionRegex);
            _categoryId = assignmentRuleModel.CategoryId;
        }

        public bool Matches(TransactionModel model)
        {
            return model.Description != null && _text.IsMatch(model.Description);
        }

        public void Assign(TransactionModel model)
        {
            model.CategoryId = _categoryId;
        }
    }
}