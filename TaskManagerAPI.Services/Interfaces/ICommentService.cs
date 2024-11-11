using TaskManagerAPI.Core.DTOs;

namespace TaskManagerAPI.Services.Interfaces;

public interface ICommentService
{
    Task AddCommentAndLogHistoryAsync(AddCommentDTO commentDto);
}