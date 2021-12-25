using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Task_Tracker_Proj.Services;
using Task_Tracker_Proj.Services.Interfaces;

namespace Task_Tracker_Proj.Services
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected RepositoryContext context;
        public Repository(RepositoryContext context)
        {
            this.context = context;
        }
        public void Create(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }
        public void Update(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
        }
        public void Delete(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }

        public IQueryable<TEntity> GetAll()
        {
            return context.Set<TEntity>().AsNoTracking();
        }

        public IQueryable<TEntity> GetByExpression(Expression<Func<TEntity, bool>> expression)
        {
            return context.Set<TEntity>().AsQueryable().Where(expression).AsNoTracking();
        }


    }
}
