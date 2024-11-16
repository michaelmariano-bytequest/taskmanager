namespace TaskManagerAPI.Core.Entities;

/// <summary>
/// Represents a history record for a task in the task manager system.
/// </summary>
public class History
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public string Description { get; set; }
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    public int UserId { get; set; }
}