using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.Models.Goals
{
    public class GoalModelMapper : IModelMapper<GoalModel, Goal>
    {
        public Goal CreateEntity(GoalModel model)
        {
            throw new NotImplementedException();
        }

        public GoalModel Map(Goal model)
        {
            throw new NotImplementedException();
        }

        public void Patch(GoalModel model, Goal entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GoalModel> MapList(IEnumerable<Goal> results)
        {
            return results.Select(Map);
        }
    }
}