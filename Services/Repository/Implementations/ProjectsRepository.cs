using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_Tracker_Proj.Services.Sorting;
using TaskTracker.Models;
using TaskTracker.Services.Interfaces;
using TaskTracker.Services.Sorting.Interfaces;

namespace TaskTracker.Services
{
    public sealed class ProjectsRepository : BaseRepository<Project>, IProjectsRepository
    {
        ISortingHelper<Project> helper;
        public ProjectsRepository(RepositoryContext context, ISortingHelper<Project> _helper) : base(context)
        {
            helper = _helper;
        }
        public async Task<List<Project>> GetAllWithTasksAsync(ProjectParameters parameters = null)
        {
            var projects = context.Set<Project>().Include(p => p.Tasks);
            var sorted_projects = helper.ApplySort(projects, parameters.OrderBy);
            var filtered_projects = (await sorted_projects.ToListAsync()).Where(p => 
            {
                if (parameters.Name != null && !p.Name.Equals(parameters.Name))
                    return false;
                if (parameters.Description != null && p.Description.Equals(parameters.Description))
                    return false;

                return true;
            }).ToList();

            return filtered_projects;
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

        public async Task<Project> GeByIdWithTasksAsNoTrackingAsync(int id)
        {
            return await context.Set<Project>()
                .AsNoTracking()
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.ProjectId.Equals(id));
        }

    }
}
