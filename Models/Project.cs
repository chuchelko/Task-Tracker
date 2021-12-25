using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaskTracker.Models
{
    public enum ProjectStatus
    {
        NotStarted,
        Active,
        Completed
    }
    public class Project
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Input Name of the Project")]
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Priority { get; set; } = 1;
        public DateTime? StartTime { get; set; } = DateTime.Now;
        public DateTime? CompletionTime { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProjectStatus? Status { get; set; } = ProjectStatus.NotStarted;
        public List<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
    }
}
