using System.Text.Json.Serialization;

namespace TaskTracker.Models.DTOs
{
    public class ProjectTaskDTOWithID : ProjectTaskDTOBase
    {
        public ProjectTaskDTOWithID(ProjectTask task) : base(task)
        {
            Id = task.ProjectTaskId;
        }
        public ProjectTaskDTOWithID()
        {

        }
        public int Id { get; set; }
    }
}
