using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
        /// Returns all stored Projects with it's tasks.
        /// </summary>
        [ProducesResponseType(typeof(List<ProjectDTOWithID>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<List<ProjectDTOWithID>>> Get()
        {
            var projects = await repository.Projects.GetAllWithTasksAsync();
            if (projects == null)
                return new NotFoundResult();
            var projectDTOs = new List<ProjectDTOWithID>();
            projects.ForEach(p => projectDTOs.Add(new ProjectDTOWithID(p)));
            return new JsonResult(projectDTOs);
        }

        /// <summary>
        /// Returns Project with recieved ID.
        /// </summary>
        /// <param name="id">ID of Project.</param>
        [ProducesResponseType(typeof(ProjectDTOWithID), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDTOWithID>> Get(int id)
        {
            Project project = await repository.Projects.GetByIdWithTasksAsync(id);
            if (project == null)
                return NotFound();
            return new JsonResult(new ProjectDTOWithID(project));
        }

        /// <summary>
        /// Adds new Project with related Tasks.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProjectDTOWithoutID), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<ActionResult<ProjectDTOWithoutID>> Post(ProjectDTOWithoutID projectDTO)
        {
            if (projectDTO == null)
                return BadRequest(ModelState);
            var project = new Project();
            project.FromDTO(projectDTO);
            repository.Tasks.CreateRange(project.Tasks);
            repository.Projects.Create(project);
            await repository.SaveAsync();
            return CreatedAtAction(nameof(Get), new { id = project.ProjectId }, new ProjectDTOWithID(project));
        }

        /// <summary>
        /// Changes all Project's values to recieved or default values.
        /// </summary>
        /// <remarks>
        /// Returns 404 status code response when there is no Project with recieved ID in the Store.<br/>
        /// When there is no Task related to the recieved Project, returns 404 status code response.
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut]
        public async Task<ActionResult> Put(ProjectDTOWithID project)
        {
            try
            {
                var new_project = new Project();
                new_project.FromDTO(project);
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
        /// Updates Project's properties by Json Patch Document.
        /// </summary>
        /// <param name="id">Id of Project.</param>
        /// <param name="patchDocument">Json Patch Document</param>
        [ProducesResponseType(typeof(ProjectDTOWithID), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPatch("{id}")]
        public async Task<ActionResult<ProjectDTOWithID>> JsonPatchProject([FromRoute] int id, [FromBody] JsonPatchDocument<Project> patchDocument)
        {
            if (patchDocument == null)
                return BadRequest();
            var project = await repository.Projects.GetByIdWithTasksAsync(id);
            patchDocument.ApplyTo(project);
            if (ModelState.IsValid == false)
                return BadRequest();
            await repository.SaveAsync();
            return Ok(new ProjectDTOWithID(project));
        }

        /// <summary>
        /// Deletes Project with related Tasks by recieved ID.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
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
