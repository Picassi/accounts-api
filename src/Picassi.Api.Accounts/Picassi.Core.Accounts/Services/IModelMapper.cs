using System.Collections.Generic;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Models.Transactions;

namespace Picassi.Core.Accounts.Services
{
    public interface IModelMapper<TModel, TEntity>
    {
        TEntity CreateEntity(TModel model);
        TModel Map(TEntity model);
        void Patch(TModel model, TEntity entity);
        IEnumerable<TModel> MapList(IEnumerable<TEntity> results);
    }
}