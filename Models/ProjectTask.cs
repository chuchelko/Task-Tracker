using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TaskTracker.Models.DTOs;

namespace TaskTracker.Models
{
    public enum TaskStatus
    {
        ToDo,
        InProgress,
        Done
    }
    public class ProjectTask
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectTaskId { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Input Name of the Task")]
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskStatus? Status { get; set; } = TaskStatus.ToDo;
        public void SetValues(ProjectTask task)
        {
            Name = task.Name;
            Description = task.Description;
            Status = task.Status;
        }
        public void FromDTO(ProjectTaskDTOWithID taskDTO)
        {
            ProjectTaskId = taskDTO.Id;
            FromDTO(taskDTO as ProjectTaskDTOBase);
        }
        public void FromDTO(ProjectTaskDTOBase taskDTO)
        {
            Name = taskDTO.Name;
            Description = taskDTO.Description;
            Status = taskDTO.Status;
        }

    }
}
