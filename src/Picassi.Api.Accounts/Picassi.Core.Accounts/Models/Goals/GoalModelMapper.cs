using AutoMapper;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.Models.Goals
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