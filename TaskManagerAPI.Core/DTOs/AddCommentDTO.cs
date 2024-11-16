namespace TaskManagerAPI.Core.DTOs;

/// <summary>
/// A Data Transfer Object (DTO) for adding a comment to a task.
/// </summary>
public class AddCommentDTO
{
    public int TaskId { get; set; }
    public int UserId { get; set; } // Id do usuário que fez o comentário
    public string CommentText { get; set; }
}