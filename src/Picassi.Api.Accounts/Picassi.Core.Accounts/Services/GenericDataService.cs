using Picassi.Core.Accounts.DAL;
using Picassi.Core.Accounts.Exceptions;
using Picassi.Utils.Data;

namespace Picassi.Core.Accounts.Services
{
    public interface IGenericDataService<TModel>
    {
        TModel Create(TModel model);
        TModel Get(int id);
        TModel Update(int id, TModel model);
        bool Delete(int id);
    }

    public abstract class GenericDataService<TModel, TEntity> : IGenericDataService<TModel> where TEntity : class 
    {
        protected readonly IModelMapper<TModel, TEntity> ModelMapper;
        protected readonly IAccountsDataContext DbContext;

        protected GenericDataService(IModelMapper<TModel, TEntity> modelMapper, IAccountsDataContext dbContext)
        {
            ModelMapper = modelMapper;
            DbContext = dbContext;
        }

        public virtual TModel Create(TModel model)
        {
            var dataModel = ModelMapper.CreateEntity(model);
            DbContext.Set<TEntity>().Add(dataModel);
            DbContext.SaveChanges();
            return ModelMapper.Map(dataModel);
        }

        public virtual TModel Get(int id)
        {
            var entity = DbContext.Set<TEntity>().Find(id);
            if (entity == null) throw new EntityNotFoundException<TEntity>(id);

            return ModelMapper.Map(entity);
        }

        public virtual TModel Update(int id, TModel model)
        {
            var entity = DbContext.Set<TEntity>().Find(id);
            if (entity == null) throw new EntityNotFoundException<TEntity>(id);

            ModelMapper.Patch(model, entity);
            DbContext.SaveChanges();
            return ModelMapper.Map(entity);
        }

        public virtual bool Delete(int id)
        {
            var entity = DbContext.Set<TEntity>().Find(id);
            if (entity == null) throw new EntityNotFoundException<TEntity>(id);

            DbContext.Set<TEntity>().Remove(entity);
            DbContext.SaveChanges();

            return true;
        }
    }
}
