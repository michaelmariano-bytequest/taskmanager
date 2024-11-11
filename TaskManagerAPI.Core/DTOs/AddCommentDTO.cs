namespace TaskManagerAPI.Core.DTOs;

public class AddCommentDTO
{
    public int TaskId { get; set; }
    public int UserId { get; set; } // Id do usuário que fez o comentário
    public string CommentText { get; set; }
}