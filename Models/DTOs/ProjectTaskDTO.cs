namespace TaskTracker.Models.DTOs
{
    public class ProjectTaskDTO
    {
        public ProjectTaskDTO(ProjectTask task)
        {
            Id = task.ProjectTaskId;
            Name = task.Name;
            Description = task.Description;
            Status = task.Status;
        }
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public TaskStatus? Status { get; } = TaskStatus.ToDo;
    }
}
