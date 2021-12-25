using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_Tracker_Proj.Models;
using Task_Tracker_Proj.Models.DTOs;
using Task_Tracker_Proj.Services.Interfaces;

namespace Task_Tracker_Proj.Controllers
{
    [Route("api/projects/{project_id}/tasks")]
    [ApiController]
    public class ProjectTasksController : ControllerBase
    {
        IRepositoryWrapper repository;
        public ProjectTasksController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }

        [HttpGet("{task_id}")]
        public async Task<ActionResult<ProjectTaskDTO>> GetTaskById(int project_id, int task_id)
        {
            var project = await repository.Projects.GetByIdWithTasksAsync(project_id);
            var task = project?.Tasks.FirstOrDefault(t => t.ProjectTaskId.Equals(task_id));
            if (task == null)
                return NotFound();
            ProjectTaskDTO taskDTO = new ProjectTaskDTO(task);
            return new JsonResult(taskDTO);
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectTaskDTO>>> GetAllTasks(int project_id)
        {
            var project = await repository.Projects.GetByIdWithTasksAsync(project_id);
            var tasks = project?.Tasks;
            if (tasks == null)
                return NotFound();
            List<ProjectTaskDTO> taskDTOs = new List<ProjectTaskDTO>();
            tasks.ForEach(t => taskDTOs.Add(new ProjectTaskDTO(t)));
            return new JsonResult(taskDTOs);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectTaskDTO>> PostTask(int project_id, ProjectTask new_task)
        {
            var project = await repository.Projects.GetByIdWithTasksAsync(project_id);
            var old_task = project.Tasks.FirstOrDefault(t => t.ProjectTaskId == new_task.ProjectTaskId);
            if (old_task != null)
                return BadRequest();
            project.Tasks.Add(new_task);
            await repository.SaveAsync();
            return CreatedAtAction(nameof(GetTaskById)
                , new { project_id = new_task.ProjectId, task_id = new_task.ProjectTaskId }
                , new ProjectTaskDTO(new_task));
        }

        [HttpPut]
        public async Task<ActionResult<ProjectTaskDTO>> PutTask(int project_id, ProjectTask task)
        {
            var old_task = (await repository.Projects.GetByIdWithTasksAsync(project_id))?.Tasks;
            if (old_task == null)
                return NotFound();
            repository.Tasks.Update(task);
            await repository.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{task_id}")]
        public async Task<ActionResult> DeleteTask(int project_id, int task_id)
        {
            var project = await repository.Projects.GetByIdWithTasksAsync(project_id);
            var task = project.Tasks.FirstOrDefault(t => t.ProjectTaskId.Equals(task_id));
            if (task == null)
                return BadRequest();
            project.Tasks.Remove(task);
            await repository.SaveAsync();
            return NoContent();
        }
    }
}
