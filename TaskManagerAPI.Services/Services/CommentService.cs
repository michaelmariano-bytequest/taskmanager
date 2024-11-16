using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Infrastructure.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services.Services;

/// Service for handling operations related to comments and logging related history.
public class CommentService : ICommentService
{
    /// <summary>
    /// Represents the repository used to manage comment-related data operations
    /// within the TaskManagerAPI. It provides methods to add and manipulate comments.
    /// </summary>
    private readonly ICommentRepository _commentRepository;

    /// <summary>
    /// Service responsible for managing history entries related to tasks.
    /// Used to log and retrieve historical actions and changes for tasks.
    /// </summary>
    private readonly IHistoryService _historyService;

    /// <summary>
    /// Service responsible for managing comments and their associated task history.
    /// </summary>
    public CommentService(ICommentRepository commentRepository, IHistoryService historyService)
    {
        _commentRepository = commentRepository;
        _historyService = historyService;
    }

    /// <summary>
    /// Adds a comment to a task and logs the history of this action.
    /// </summary>
    /// <param name="commentDto">The DTO containing comment details such as TaskId, UserId, and CommentText.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task AddCommentAndLogHistoryAsync(AddCommentDTO commentDto)
    {
        // Adiciona o comentário à tarefa
        var comment = new Comment
        {
            TaskId = commentDto.TaskId,
            UserId = commentDto.UserId,
            CommentText = commentDto.CommentText,
            CreatedAt = DateTime.UtcNow
        };
        
        await _commentRepository.AddCommentAsync(comment);

        // Adiciona o comentário ao histórico da tarefa
        var historyDescription = $"Comment added by user {commentDto.UserId}: {commentDto.CommentText}";
        await _historyService.AddHistoryEntryAsync(commentDto.TaskId, historyDescription, commentDto);
    }
}