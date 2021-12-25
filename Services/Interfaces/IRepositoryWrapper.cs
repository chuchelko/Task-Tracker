using System.Threading.Tasks;
using TaskTracker.Models;

namespace TaskTracker.Services.Interfaces
{
    public interface IRepositoryWrapper
    {
        IProjectsRepository Projects { get; }
        ITasksRepository Tasks { get; }
        Task SaveAsync();
        Task UpdateProject(Project task);
    }
}
