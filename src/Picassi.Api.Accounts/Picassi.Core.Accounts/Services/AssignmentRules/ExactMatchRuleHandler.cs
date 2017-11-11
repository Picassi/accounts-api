using System;
using Picassi.Api.Accounts.Contract.Transactions;
using Picassi.Core.Accounts.Models.AssignmentRules;

namespace Picassi.Core.Accounts.Services.AssignmentRules
{
    public class ExactMatchRuleHandler : IAssignmentRuleHandler
    {
        public int? Priority { get; }

        private readonly string _text;
        private readonly int? _categoryId;

        public ExactMatchRuleHandler(AssignmentRuleModel assignmentRuleModel)
        {
            Priority = assignmentRuleModel.Priority;
            _text = assignmentRuleModel.DescriptionRegex;
            _categoryId = assignmentRuleModel.CategoryId;
        }

        public bool Matches(ITransactionInformation model)
        {
            return model.Description != null &&
                   model.Description.Equals(_text, StringComparison.CurrentCultureIgnoreCase);
        }

        public void Assign(ITransactionInformation model)
        {
            model.CategoryId = _categoryId;
        }
    }
}