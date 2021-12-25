using System.Threading.Tasks;
using Task_Tracker_Proj.Models;

namespace Task_Tracker_Proj.Services.Interfaces
{
    public interface IRepositoryWrapper
    {
        IProjectsRepository Projects { get; }
        ITasksRepository Tasks { get; }
        Task SaveAsync();
        Task UpdateProject(Project task);
    }
}
