using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_Tracker_Proj.Models;

namespace Task_Tracker_Proj.Repositories
{
    interface IRepository<Model> where Model : BaseModel
    {
        List<Model> GetAll();
        void AddModel(Model model);
        void UpdateOrAddModel(Model model);
        Model GetModel(Guid Id);
        bool DeleteModel(Guid model);
    }
}
