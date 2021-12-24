using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_Tracker_Proj.Models;
using Task_Tracker_Proj.Models.DTOs;
using Task_Tracker_Proj.Repositories;

namespace Task_Tracker_Proj.Controllers
{
    [Route("api/projects/{project_id}")]
    [ApiController]
    public class ProjectTasksController : ControllerBase
    {
        RepositoryContext context = new RepositoryContext();
        public ProjectTasksController(RepositoryContext repository)
        {
            context = repository;
        }

        [HttpGet("{task_id}")]
        public async Task<ActionResult<ProjectTaskDTO>> GetTaskById(int project_id, int task_id)
        {
            var task = await context.Tasks.Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.ProjectId == project_id && t.ProjectTaskId == task_id);
            if (task == null)
                return NotFound();
            ProjectTaskDTO taskDTO = new ProjectTaskDTO(task);
            return new JsonResult(taskDTO);
        }

        //[HttpGet]
        //public async Task<ActionResult<ProjectTask>> GetAllTasks(int project_id)
        //{

        //}
    }
}
