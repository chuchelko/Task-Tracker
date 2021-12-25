using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using TaskTracker.Models;
using TaskTracker.Models.DTOs;
using TaskTracker.Services.Interfaces;

namespace TaskTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class ProjectsController : ControllerBase
    {
        IRepositoryWrapper repository;
        public ProjectsController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Returns all stored projects with it's tasks.
        /// </summary>
        [ProducesResponseType(typeof(Project), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<List<Project>>> Get()
        {
            var projects = await repository.Projects.GetAllWithTasksAsync();
            if (projects == null)
                return new NotFoundResult();
            var projectDTOs = new List<ProjectDTO>();
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
            repository.Tasks.CreateRange(project.Tasks);
            repository.Projects.Create(project);
            await repository.SaveAsync();
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
            try
            {
                await repository.UpdateProject(new_project);
                await repository.SaveAsync();
                return NoContent();
            }
            catch (NullReferenceException exp)
            {
                return NotFound(exp.Message);
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
            Project project = await repository.Projects.GetByIdWithTasksAsync(id);
            if (project == null)
                return NotFound();
            return new JsonResult(new ProjectDTO(project));
        }

        /// <summary>
        /// Deletes Project with related Tasks by recieved ID.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Project>> Delete(int id)
        {
            Project project = await repository.Projects.GetByIdAsync(id);
            if (project == null)
                return BadRequest();
            repository.Projects.Delete(project);
            await repository.SaveAsync();
            return NoContent();
        }

    }
}
