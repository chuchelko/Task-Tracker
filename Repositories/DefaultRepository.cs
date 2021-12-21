using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_Tracker_Proj.Models;

namespace Task_Tracker_Proj.Repositories
{
    internal class DefaultRepository<Model>: IRepository<Model> where Model : BaseModel
    {
        private RepositoryContext Context;
        public DefaultRepository(RepositoryContext context)
        {
            Context = context;
        }
        public void AddModel(Model model)
        {
            Context.Add(model);
            Context.SaveChanges();
        }

        public bool DeleteModel(Guid id)
        {
            var model = GetModel(id);
            if (model == null)
                return false;
            Context.Set<Model>().Remove(GetModel(id));
            Context.SaveChanges();
            return true;
        }

        public List<Model> GetAll()
        {
            return Context.Set<Model>().ToList();
        }
        public Model GetModel(Guid id)
        {
            return Context.Set<Model>().FirstOrDefault(x => x.Id == id);
        }

        public void UpdateOrAddModel(Model model)
        {
            if (GetModel(model.Id) == null)
                Context.Add(model);
            else
                Context.Update(model);
            Context.SaveChanges();
        }
    }
}
