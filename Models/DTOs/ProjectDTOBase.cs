using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaskTracker.Models.DTOs
{
    public abstract class ProjectDTOBase
    {
        public ProjectDTOBase()
        {

        }
        public ProjectDTOBase(Project project)
        {
            Name = project.Name;
            Description = project.Description;
            Priority = project.Priority;
            StartTime = project.StartTime;
            CompletionTime = project.CompletionTime;
            Status = project.Status;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Priority { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? CompletionTime { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProjectStatus? Status { get; set; }
    }
}
