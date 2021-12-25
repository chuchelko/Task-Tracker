using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskTracker.Models;
using TaskTracker.Services.Interfaces;

namespace TaskTracker.Services
{
    public sealed class TasksRepository : BaseRepository<ProjectTask>, ITasksRepository
    {
        public TasksRepository(RepositoryContext context) : base(context)
        {
            
        }

        public async Task<List<ProjectTask>> GetAllAsync()
        {
            return await context.Set<ProjectTask>().ToListAsync();
        }

        public async Task<ProjectTask> GetByIdAsync(int id)
        {
            return await context.Set<ProjectTask>()
                .FirstOrDefaultAsync(t => t.ProjectTaskId.Equals(id));
        }

        public async Task<ProjectTask> GetByIdWithProjectAsync(int id)
        {
            return await context.Set<ProjectTask>()
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.ProjectTaskId.Equals(id));
        }
    }
}
