using AutoMapper;
using Picassi.Core.Accounts.Services;
using Picassi.Core.Accounts.ViewModels.Goals;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.DbAccess
{
    public class GoalModelMapper : IModelMapper<GoalModel, Goal>
    {
        public Goal CreateEntity(GoalModel model)
        {
            return Mapper.Map<Goal>(model);
        }

        public GoalModel Map(Goal model)
        {
            return Mapper.Map<GoalModel>(model);
        }

        public void Patch(GoalModel model, Goal entity)
        {
            Mapper.Map(model, entity);
        }
    }
}