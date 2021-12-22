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
            using(RepositoryContext context = new RepositoryContext())
            {
                var old_project = context.Projects.Include(p => p.Tasks)
                    .FirstOrDefault(p => p.ProjectId == new_project.ProjectId);
                if (old_project == null)
                {
                    foreach (var task in new_project.Tasks)
                        task.ProjectTaskId = default;
                    new_project.ProjectId = default;
                    context.Add(new_project);
                    await context.SaveChangesAsync();
                    return CreatedAtAction(nameof(Get), new { id = new_project.ProjectId }, new_project);
                }
                foreach (var task in new_project.Tasks)
                {
                    var old_task = old_project.Tasks.FirstOrDefault(t => t.ProjectTaskId == task.ProjectTaskId);
                    if (old_task == null)
                        return BadRequest();
                    old_task.SetValues(task); //здесь можно передавать контекст и менять EntityState
                    context.Entry(old_task).State = EntityState.Modified;
                }
                foreach (var task_to_delete in old_project.Tasks)
                    if (context.Entry(task_to_delete).State == EntityState.Unchanged)
                        context.Remove(task_to_delete);
                
                context.Entry(old_project).CurrentValues.SetValues(new_project);
                Console.WriteLine(context.ChangeTracker.DebugView.LongView);
                await context.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { id = old_project.ProjectId }, old_project);
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
