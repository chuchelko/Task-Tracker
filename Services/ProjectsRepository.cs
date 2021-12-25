using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskTracker.Models;
using TaskTracker.Services.Interfaces;

namespace TaskTracker.Services
{
    public sealed class ProjectsRepository : BaseRepository<Project>, IProjectsRepository
    {
        public ProjectsRepository(RepositoryContext context) : base(context)
        { 

        }

        public async Task<List<Project>> GetAllWithTasksAsync()
        {
            return await context.Set<Project>().Include(p => p.Tasks).ToListAsync();
        }

        public async Task<Project> GetByIdAsync(int id)
        {
            return await context.Set<Project>()
                .FirstOrDefaultAsync(p => p.ProjectId.Equals(id));
        }

        public async Task<Project> GetByIdWithTasksAsync(int id)
        {
            return await context.Set<Project>()
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.ProjectId.Equals(id));
        }

        
        
    }
}
