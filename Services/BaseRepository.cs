using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskTracker.Services
{
    public abstract class BaseRepository<TEntity> where TEntity : class
    {
        protected RepositoryContext context;
        public BaseRepository(RepositoryContext context)
        {
            this.context = context;
        }
        public virtual void Create(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }
        public virtual void Update(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
        }
        public virtual void Delete(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }
        public virtual void CreateRange(List<TEntity> entities)
        {
            context.Set<TEntity>().AddRange(entities);
        }
    }
}
