using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Task_Tracker_Proj.Models;
using Task_Tracker_Proj.Repositories;

namespace Task_Tracker_Proj.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        ILogger<ApiController> _logger;
        RepositoryContext _repository = new RepositoryContext();
        public ApiController(ILogger<ApiController> logger, RepositoryContext repository)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("projects")]
        public async Task<ActionResult<List<Project>>> Get()
        {
            var projects = await _repository.Projects.Include(p => p.Tasks).ToListAsync();
            if (projects == null)
                return new NoContentResult();
            return new JsonResult(projects);
        }

        [HttpPost("projects")]
        public async Task<ActionResult<Project>> Post(Project project)
        {
            if (project == null)
                return BadRequest(ModelState);
            _repository.Tasks.AddRange(project.Tasks);
            _repository.Projects.Add(project);
            await _repository.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = project.ProjectId }, project);
        }

        [HttpPut("projects")]
        public async Task<ActionResult<Project>> Put(Project new_project)
        {
            //_repository.Entry(new_project).State = EntityState.Detached;
            //project = new_project;
            //_repository.UpdateRange(project.Tasks);
            //_repository.Update(new_project);
            //_repository.Database.EnsureDeleted();
            //_repository.Database.EnsureCreated();

            using (RepositoryContext context = new RepositoryContext())
            {
                var project = context.Projects.AsNoTracking().Include(p => p.Tasks).FirstOrDefault(p => p.ProjectId == new_project.ProjectId);
                if (project == null)
                {
                    new_project.ProjectId = default;
                    foreach (var task in new_project.Tasks)
                    {
                        task.ProjectTaskId = default;
                    }
                    context.Projects.Add(new_project);
                }
                else
                {
                    foreach (var task in new_project.Tasks)
                    {
                        var old_task = context.Tasks.Include(t => t.Project).FirstOrDefault(t => t.ProjectTaskId == task.ProjectTaskId);

                        if (old_task == null)
                        {
                            task.ProjectTaskId = default;
                            context.Tasks.Add(task);
                        }
                        else
                        {
                            task.Project = project;
                            context.Entry(old_task).CurrentValues.SetValues(task);
                        }
                            

                    }
                    context.Entry(project).CurrentValues.SetValues(new_project);

                }

                Console.WriteLine(context.ChangeTracker.DebugView.LongView);

                foreach (var task in project.Tasks)
                {
                    task.Project = project;
                }
                ////Add tasks related to this project
                //foreach (var task in new_project.Tasks)
                //{
                //    var old_task = context.Tasks.FirstOrDefault(t => t.ProjectTaskId == task.ProjectTaskId);
                //    if (old_task == null)
                //    {
                //        //Put request adds task with new unique ID when there is no task in DB
                //        //having adding task's id
                //        task.ProjectTaskId = default;
                //        context.Tasks.Add(task);
                //    }
                //    else
                //        context.Entry(old_task).CurrentValues.SetValues(task);
                //}
                
                await context.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { id = new_project.ProjectId }, new_project);
            }


        }

        [HttpGet("projects/{id}")]
        public async Task<ActionResult<Project>> Get(int id)
        {
            Project proj = await _repository.Projects.Include("Tasks").FirstOrDefaultAsync(x => x.ProjectId == id);
            if (proj == null)
                return NotFound();
            return new JsonResult(proj);
        }


        //works
        [HttpDelete("projects/{id}")]
        public async Task<ActionResult<Project>> Delete(int id)
        {
            Project project = _repository.Projects.Include("Tasks").FirstOrDefault(x => x.ProjectId == id);
            if (project == null)
                return NotFound();
            _repository.Projects.Remove(project);
            await _repository.SaveChangesAsync();
            return Ok(project);
        }
        //works
        [HttpDelete("projects/")]
        public async Task<ActionResult<Project>> DeleteAll(int id)
        {
            var project = _repository.Projects.Include("Tasks");
            _repository.Projects.RemoveRange(project);
            await _repository.SaveChangesAsync();
            return Ok(project);
        }

    }
}
