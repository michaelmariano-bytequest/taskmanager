using TaskManagerAPI.Core.Entities;

namespace TaskManagerAPI.Infrastructure.Interfaces;

/// <summary>
/// Interface for managing comments within the TaskManagerAPI.
/// </summary>
public interface ICommentRepository
{
    Task AddCommentAsync(Comment comment);
}