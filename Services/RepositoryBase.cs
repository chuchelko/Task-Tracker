using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Task_Tracker_Proj.Repositories;
using Task_Tracker_Proj.Services.Interfaces;

namespace Task_Tracker_Proj.Services
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        RepositoryContext context;
        public RepositoryBase(RepositoryContext context)
        {
            this.context = context;
        }
        public void Create(T entity)
        {
            context.
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
