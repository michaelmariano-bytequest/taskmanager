using TaskManagerAPI.Core.DTOs;

namespace TaskManagerAPI.Services.Interfaces;

/// Interface for managing comment operations and their related history.
/// /
public interface ICommentService
{
    /// <summary>
    /// Adds a comment to a task and logs the action in the task's history.
    /// </summary>
    /// <param name="commentDto">The data transfer object containing the details of the comment to be added.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddCommentAndLogHistoryAsync(AddCommentDTO commentDto);
}