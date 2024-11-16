namespace TaskManagerAPI.Core.Entities;

/// <summary>
/// Represents a comment made on a task in the TaskManager system.
/// </summary>
public class Comment
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public int UserId { get; set; } // Id do usuário que fez o comentário
    public string CommentText { get; set; }
    public DateTime CreatedAt { get; set; }
}