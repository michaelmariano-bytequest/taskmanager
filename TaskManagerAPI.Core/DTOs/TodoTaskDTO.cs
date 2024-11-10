using TaskManagerAPI.Core.Enums;

namespace TaskManagerAPI.Core.DTOs;

public class TodoTaskDTO
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DueDate { get; set; }
    public TodoTaskPriorityEnum Priority { get; set; }
    public TodoTaskStatusEnum Status { get; set; }
}