using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Api.Accounts.Contract.Enums;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.AssignmentRules;

namespace Picassi.Core.Accounts.Services.AssignmentRules
{
    public interface IAssignmentRuleProvider
    {
        IEnumerable<IAssignmentRuleHandler> GetAssignmentRules();
    }

    public class AssignmentRuleProvider : IAssignmentRuleProvider
    {
        private readonly IAssignmentRulesDataService _assignmentRulesDataService;
        private IEnumerable<AssignmentRuleModel> _rules = null;

        public AssignmentRuleProvider(IAssignmentRulesDataService assignmentRulesDataService)
        {
            _assignmentRulesDataService = assignmentRulesDataService;
        }

        public IEnumerable<IAssignmentRuleHandler> GetAssignmentRules()
        {
            if (_rules == null)
            {
                _rules = _assignmentRulesDataService.GetRules(enabled: true);
            }

            return _rules.Select(BuildRuleHandler);
        }

        private IAssignmentRuleHandler BuildRuleHandler(AssignmentRuleModel arg)
        {
            switch (arg.Type)
            {
                case AssignmentRuleType.ExactMatch:
                    return new ExactMatchRuleHandler(arg);
                case AssignmentRuleType.PartialMatch:
                    return new FuzzyMatchRuleHandler(arg);
                case AssignmentRuleType.Regex:
                    return new RegexMatchRuleHandler(arg);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
