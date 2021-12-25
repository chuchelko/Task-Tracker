using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Task_Tracker_Proj.Services.Interfaces
{
    interface IRepositoryBase<T>
    {
        IQueryable<T> Get(Expression<Func<T, bool>> expression);
        void Update(T entity);
        void Create(T entity);
        void Delete(T entity);
    }
}
