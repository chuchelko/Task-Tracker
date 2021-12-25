using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Task_Tracker_Proj.Models;
using Task_Tracker_Proj.Services.Interfaces;

namespace Task_Tracker_Proj.Services
{
    public sealed class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext context { get; }
        public RepositoryWrapper(RepositoryContext context)
        {
            this.context = context;
        }
        public IProjectsRepository projects;
        public ITasksRepository tasks;

        public IProjectsRepository Projects 
        {
            get
            {
                if (projects == null)
                    projects = new ProjectsRepository(context);
                return projects;
            }
        }
        public ITasksRepository Tasks
        {
            get
            {
                if (tasks == null)
                    tasks = new TasksRepository(context);
                return tasks;
            }
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
        public async Task UpdateProject(Project new_project)
        {
            var old_project = await Projects.GetByIdWithTasksAsync(new_project.ProjectId);
            if (old_project == null)
                throw new NullReferenceException($"There is no Project with id {new_project.ProjectId} in the Store");
            foreach (var task in new_project.Tasks)
            {
                var old_task = old_project.Tasks.FirstOrDefault(t => t.ProjectTaskId == task.ProjectTaskId);
                if (old_task == null)
                    throw new NullReferenceException($"There is no Task with id {task.ProjectTaskId} in the Store");
                old_task.SetValues(task);
                context.Entry(old_task).State = EntityState.Modified;
            }
            foreach (var task_to_delete in old_project.Tasks)
                if (context.Entry(task_to_delete).State == EntityState.Unchanged)
                    context.Remove(task_to_delete);

            context.Entry(old_project).CurrentValues.SetValues(new_project);
        }
    }
}
