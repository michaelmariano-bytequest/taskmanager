using TaskManagerAPI.Core.Enums;

namespace TaskManagerAPI.Core.DTOs;

/// <summary>
/// Data Transfer Object for creating a new TodoTask.
/// </summary>
public class TodoTaskCreateDTO
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DueDate { get; set; }
    public TodoTaskPriorityEnum Priority { get; set; }
    public TodoTaskStatusEnum Status { get; set; }
}