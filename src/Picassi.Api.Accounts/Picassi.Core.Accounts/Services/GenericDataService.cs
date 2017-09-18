using Picassi.Core.Accounts.DAL;
using Picassi.Core.Accounts.Exceptions;

namespace Picassi.Core.Accounts.Services
{
    public interface IGenericDataService<TModel>
    {
        TModel Create(TModel model);
        TModel Get(int id);
        TModel Update(int id, TModel model);
        bool Delete(int id);
    }

    public abstract class GenericDataService<TModel, TEntity> : IGenericDataService<TModel> where TEntity : class, IEntity
    {
        protected readonly IModelMapper<TModel, TEntity> ModelMapper;
        protected readonly IAccountsDatabaseProvider DbProvider;

        protected GenericDataService(IModelMapper<TModel, TEntity> modelMapper, IAccountsDatabaseProvider dbProvider)
        {
            ModelMapper = modelMapper;
            DbProvider = dbProvider;
        }

        public virtual TModel Create(TModel model)
        {
            var dataModel = ModelMapper.CreateEntity(model);
            DbProvider.GetDataContext().GetDbSet<TEntity>().Add(dataModel);
            DbProvider.GetDataContext().SaveChanges();
            return Get(dataModel.Id);
        }

        public virtual TModel Get(int id)
        {
            var entity = DbProvider.GetDataContext().GetDbSet<TEntity>().Find(id);
            if (entity == null) throw new EntityNotFoundException<TEntity>(id);

            return ModelMapper.Map(entity);
        }

        public virtual TModel Update(int id, TModel model)
        {
            var entity = DbProvider.GetDataContext().GetDbSet<TEntity>().Find(id);
            if (entity == null) throw new EntityNotFoundException<TEntity>(id);

            ModelMapper.Patch(model, entity);
            DbProvider.GetDataContext().SaveChanges();
            return Get(id);
        }

        public virtual bool Delete(int id)
        {
            var entity = DbProvider.GetDataContext().GetDbSet<TEntity>().Find(id);
            if (entity == null) throw new EntityNotFoundException<TEntity>(id);

            DbProvider.GetDataContext().GetDbSet<TEntity>().Remove(entity);
            DbProvider.GetDataContext().SaveChanges();

            return true;
        }
    }
}
