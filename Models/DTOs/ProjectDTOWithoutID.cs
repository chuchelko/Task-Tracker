using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaskTracker.Models.DTOs
{
    public class ProjectDTOWithoutID : ProjectDTOBase
    {
        public ProjectDTOWithoutID()
        {

        }
        public ProjectDTOWithoutID(Project project) : base(project)
        {
            project.Tasks.ForEach(t =>
            {
                TasksWithoutID.Add(new ProjectTaskDTOBase(t));
            });
        }
        public List<ProjectTaskDTOBase> TasksWithoutID { get; set; } = new List<ProjectTaskDTOBase>();
    }
}
