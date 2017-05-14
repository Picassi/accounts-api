namespace Picassi.Core.Accounts.Services
{
    public interface IModelMapper<TModel, TEntity>
    {
        TEntity CreateEntity(TModel model);
        TModel Map(TEntity model);
        void Patch(TModel model, TEntity entity);
    }
}