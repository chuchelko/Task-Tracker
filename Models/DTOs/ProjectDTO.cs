using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_Tracker_Proj.Models.DTOs
{
    public class ProjectDTO
    {
        public ProjectDTO(Project project)
        {
            Id = project.ProjectId;
            Name = project.Name;
            Description = project.Description;
            Priority = project.Priority;
            StartTime = project.StartTime;
            CompletionTime = project.CompletionTime;
            Status = project.Status;
            project.Tasks.ForEach(t => Tasks.Add(new ProjectTaskDTO(t)));
        }
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public int? Priority { get; }
        public DateTime? StartTime { get; }
        public DateTime? CompletionTime { get; }
        public ProjectStatus? Status { get; }
        public List<ProjectTaskDTO> Tasks { get; set; } = new List<ProjectTaskDTO>();
    }
}
