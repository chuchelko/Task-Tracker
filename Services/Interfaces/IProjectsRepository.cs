using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_Tracker_Proj.Models;

namespace Task_Tracker_Proj.Services.Interfaces
{
    public interface IProjectsRepository
    {
        Task<List<Project>> GetAllWithTasksAsync();
        Task<Project> GetByIdAsync(int id);
        Task<Project> GetByIdWithTasksAsync(int id);
        void Create(Project project);
        void CreateRange(List<Project> projects);
        void Update(Project project);
        void Delete(Project project);
    }
}
