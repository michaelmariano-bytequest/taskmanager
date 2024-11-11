using TaskManagerAPI.Core.Entities;

namespace TaskManagerAPI.Infrastructure.Interfaces;

public interface ICommentRepository
{
    Task AddCommentAsync(Comment comment);
}