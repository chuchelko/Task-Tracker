using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_Tracker_Proj.Models;

namespace Task_Tracker_Proj.Services.Interfaces
{
    public interface ITasksRepository
    {
        Task<List<ProjectTask>> GetAllAsync();
        Task<ProjectTask> GetByIdAsync(int id);
        Task<ProjectTask> GetByIdWithProjectAsync(int id);
        void Create(ProjectTask task);
        void CreateRange(List<ProjectTask> tasks);
        void Update(ProjectTask task);
        void Delete(ProjectTask task);
    }
}
