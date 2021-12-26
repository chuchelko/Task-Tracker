using System.Text.Json.Serialization;

namespace TaskTracker.Models.DTOs
{
    public class ProjectTaskDTOBase
    {
        public ProjectTaskDTOBase(ProjectTask task)
        {
            Name = task.Name;
            Description = task.Description;
            Status = task.Status;
        }
        public ProjectTaskDTOBase()
        {

        }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskStatus? Status { get; set; } = TaskStatus.ToDo;
    }
}
