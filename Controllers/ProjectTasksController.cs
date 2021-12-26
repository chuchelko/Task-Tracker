using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using TaskTracker.Models;
using TaskTracker.Models.DTOs;
using TaskTracker.Services.Interfaces;

namespace TaskTracker.Controllers
{
    [Route("api/projects/{project_id}/tasks")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class ProjectTasksController : ControllerBase
    {
        IRepositoryWrapper repository;
        public ProjectTasksController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Returns Task by it's and Project's IDs
        /// </summary>
        /// <param name="project_id">ID of Project</param>
        /// <param name="task_id">ID of Task</param>
        [ProducesResponseType(typeof(List<ProjectTaskDTOWithID>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{task_id}")]
        public async Task<ActionResult<ProjectTaskDTOWithID>> GetTaskById(int project_id, int task_id)
        {
            var project = await repository.Projects.GetByIdWithTasksAsync(project_id);
            var task = project?.Tasks.FirstOrDefault(t => t.ProjectTaskId.Equals(task_id));
            if (task == null)
                return NotFound();
            ProjectTaskDTOWithID taskDTO = new ProjectTaskDTOWithID(task);
            return new JsonResult(taskDTO);
        }

        /// <summary>
        /// Returns Tasks by Project's ID
        /// </summary>
        /// <param name="project_id">ID of Project</param>
        [ProducesResponseType(typeof(List<ProjectTaskDTOWithID>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<List<ProjectTaskDTOWithID>>> GetAllTasks(int project_id)
        {
            var project = await repository.Projects.GetByIdWithTasksAsync(project_id);
            var tasks = project?.Tasks;
            if (tasks == null)
                return NotFound();
            List<ProjectTaskDTOWithID> taskDTOs = new List<ProjectTaskDTOWithID>();
            tasks.ForEach(t => taskDTOs.Add(new ProjectTaskDTOWithID(t)));
            return new JsonResult(taskDTOs);
        }

        /// <summary>
        /// Adds new Task for the Project with recieved ID
        /// </summary>
        /// <param name="project_id">ID of Project</param>
        /// <param name="taskDTO">Adding Task</param>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProjectTaskDTOWithID), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<ActionResult<ProjectTaskDTOWithID>> PostTask(int project_id, [FromBody] ProjectTaskDTOBase taskDTO)
        {
            if (taskDTO == null)
                return BadRequest();
            var project = await repository.Projects.GetByIdWithTasksAsync(project_id);
            if (project == null)
                return BadRequest();
            var new_task = new ProjectTask() { ProjectId = project_id };
            new_task.FromDTO(taskDTO);
            project.Tasks.Add(new_task);
            await repository.SaveAsync();
            return CreatedAtAction(nameof(GetTaskById)
                , new { project_id = new_task.ProjectId, task_id = new_task.ProjectTaskId }
                , new ProjectTaskDTOWithID(new_task));
        }

        /// <summary>
        /// Changes Task's values to recieved or default values.
        /// </summary>
        /// <remarks>
        /// Returns 404 status code response when there is no Project with recieved ID in the Store.<br/>
        /// When there is no Task with recieved ID related to the recieved Project, returns 404 status code response.
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{task_id}")]
        public async Task<ActionResult> PutTask(int project_id, int task_id, ProjectTaskDTOBase taskDTO)
        {
            var old_task = (await repository.Projects.GeByIdWithTasksAsNoTrackingAsync(project_id))?.Tasks
                .FirstOrDefault(t => t.ProjectTaskId.Equals(task_id));
            if (old_task == null)
                return NotFound();
            var new_task = new ProjectTask() { ProjectTaskId = task_id, ProjectId = project_id };
            new_task.FromDTO(taskDTO);
            repository.Tasks.Update(new_task);
            await repository.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// Deletes Task from the Store with recieved Project's and Task's ID.
        /// </summary>
        /// <remarks>
        /// Returns 400 status code response when there is no Project with recieved ID in the Store.<br/>
        /// When there is no Task related to the recieved Project, returns 400 status code response.
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{task_id}")]
        public async Task<ActionResult> DeleteTask(int project_id, int task_id)
        {
            var project = await repository.Projects.GetByIdWithTasksAsync(project_id);
            var task = project?.Tasks.FirstOrDefault(t => t.ProjectTaskId.Equals(task_id));
            if (task == null)
                return BadRequest();
            project.Tasks.Remove(task);
            await repository.SaveAsync();
            return NoContent();
        }
    }
}
