using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Task_Tracker_Proj.Models;
using Task_Tracker_Proj.Models.DTOs;
using Task_Tracker_Proj.Repositories;

namespace Task_Tracker_Proj.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class ProjectsController : ControllerBase
    {
        RepositoryContext _repository = new RepositoryContext();
        public ProjectsController(RepositoryContext repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Returns all stored projects with it's tasks.
        /// </summary>
        [ProducesResponseType(typeof(Project), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<List<Project>>> Get()
        {
            var projects = await _repository.Projects.Include(p => p.Tasks).ToListAsync();
            if (projects == null)
                return new NotFoundResult();
            List<ProjectDTO> projectDTOs = new List<ProjectDTO>();
            projects.ForEach(p => projectDTOs.Add(new ProjectDTO(p)));
            return new JsonResult(projectDTOs);
        }

        /// <summary>
        /// Adds new project with related tasks.
        /// </summary>
        [ProducesResponseType(typeof(Project), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<ActionResult<ProjectDTO>> Post(Project project)
        {
            if (project == null)
                return BadRequest(ModelState);
            _repository.Tasks.AddRange(project.Tasks);
            _repository.Projects.Add(project);
            await _repository.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = project.ProjectId }, new ProjectDTO(project));
        }

        /// <summary>
        /// Changes all Project's value to recieved or default values.
        /// </summary>
        /// <remarks>
        /// Returns 404 status code response when there is no Project with recieved ID in Database.<br/>
        /// When there is no Task related to the recieved Project, returns 404 status code response.
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut]
        public async Task<ActionResult<Project>> Put(Project new_project)
        {
            using (RepositoryContext context = new RepositoryContext())
            {
                var old_project = context.Projects.Include(p => p.Tasks)
                    .FirstOrDefault(p => p.ProjectId == new_project.ProjectId);
                if (old_project == null)
                    return NotFound();
                foreach (var task in new_project.Tasks)
                {
                    var old_task = old_project.Tasks.FirstOrDefault(t => t.ProjectTaskId == task.ProjectTaskId);
                    if (old_task == null)
                        return NotFound();
                    old_task.SetValues(task); //здесь можно передавать контекст и менять EntityState
                    context.Entry(old_task).State = EntityState.Modified;
                }
                foreach (var task_to_delete in old_project.Tasks)
                    if (context.Entry(task_to_delete).State == EntityState.Unchanged)
                        context.Remove(task_to_delete);

                context.Entry(old_project).CurrentValues.SetValues(new_project);
                Console.WriteLine(context.ChangeTracker.DebugView.LongView);
                await context.SaveChangesAsync();
                return NoContent();
            }
        }

        /// <summary>
        /// Returns Project with recieved ID.
        /// </summary>
        /// <param name="id">ID of the Project.</param>
        [ProducesResponseType(typeof(Project), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> Get(int id)
        {
            Project project = await _repository.Projects.Include("Tasks").FirstOrDefaultAsync(x => x.ProjectId == id);
            if (project == null)
                return NotFound();
            return new JsonResult(new ProjectDTO(project));
        }


        //works
        /// <summary>
        /// Deletes Project with related Tasks by recieved ID.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Project>> Delete(int id)
        {
            Project project = _repository.Projects.Include("Tasks").FirstOrDefault(x => x.ProjectId == id);
            if (project == null)
                return BadRequest();
            _repository.Projects.Remove(project);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
        //works
        //[HttpDelete("projects/")]
        //public async Task<ActionResult<Project>> DeleteAll(int id)
        //{
        //    var project = _repository.Projects.Include("Tasks");
        //    _repository.Projects.RemoveRange(project);
        //    await _repository.SaveChangesAsync();
        //    return Ok(project);
        //}

    }
}
