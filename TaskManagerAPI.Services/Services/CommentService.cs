using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Infrastructure.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IHistoryService _historyService;

    public CommentService(ICommentRepository commentRepository, IHistoryService historyService)
    {
        _commentRepository = commentRepository;
        _historyService = historyService;
    }

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