using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.Models.AssignmentRules
{
    public interface IAssignmentRuleModelMapper : IModelMapper<AssignmentRuleModel, AssignmentRule>
    {

    }

    public class AssignmentRuleModelMapper : IAssignmentRuleModelMapper
    {
        public AssignmentRule CreateEntity(AssignmentRuleModel model)
        {
            return new AssignmentRule
            {
                CreatedDate = model.CreatedDate,
                DescriptionRegex = model.DescriptionRegex,
                Enabled = model.Enabled,
                Priority = model.Priority,
                CategoryId = model.CategoryId,
                Type = model.Type
            };
        }

        public AssignmentRuleModel Map(AssignmentRule model)
        {
            return new AssignmentRuleModel
            {
                Id = model.Id,
                CreatedDate = model.CreatedDate,
                DescriptionRegex = model.DescriptionRegex,
                Enabled = model.Enabled,
                Priority = model.Priority,
                CategoryId = model.CategoryId,
                CategoryName = model.Category?.Name,
                Type = model.Type
            };
        }

        public void Patch(AssignmentRuleModel model, AssignmentRule entity)
        {
            entity.CreatedDate = model.CreatedDate;
            entity.DescriptionRegex = model.DescriptionRegex;
            entity.Enabled = model.Enabled;
            entity.Priority = model.Priority;
            entity.CategoryId = model.CategoryId;
            entity.Type = model.Type;
        }

        public IEnumerable<AssignmentRuleModel> MapList(IEnumerable<AssignmentRule> results)
        {
            return results.Select(Map);
        }
    }
}
