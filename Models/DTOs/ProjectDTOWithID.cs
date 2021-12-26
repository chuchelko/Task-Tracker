using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaskTracker.Models.DTOs
{
    public class ProjectDTOWithID : ProjectDTOBase
    {
        public ProjectDTOWithID()
        {

        }
        public ProjectDTOWithID(Project project) : base(project)
        {
            Id = project.ProjectId;
            project.Tasks.ForEach(t =>
            {
                TasksWithID.Add(new ProjectTaskDTOWithID(t));
            });
        }
        public int Id { get; set; }
        public List<ProjectTaskDTOWithID> TasksWithID { get; set; } = new List<ProjectTaskDTOWithID>();
    }
}
