using TaskManagerAPI.Core.Enums;

namespace TaskManagerAPI.Core.Entities;

/// <summary>
/// Represents a task in the to-do list.
/// </summary>
public class TodoTask
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime DueDate { get; set; }
    public TodoTaskPriorityEnum Priority { get; set; }
    public TodoTaskStatusEnum Status { get; set; }
}