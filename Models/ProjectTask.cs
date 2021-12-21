using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Task_Tracker_Proj.Models
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
        

    }
}
