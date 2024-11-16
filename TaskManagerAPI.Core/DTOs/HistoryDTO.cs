namespace TaskManagerAPI.Core.DTOs;

/// <summary>
/// Data Transfer Object representing the history of changes made to a task.
/// </summary>
public class HistoryDTO
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public string Description { get; set; }
    public DateTime ModifiedAt { get; set; }
}