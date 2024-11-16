using TaskManagerAPI.Core.Entities;

namespace TaskManagerAPI.Infrastructure.Interfaces;

/// <summary>
/// Interface for User repository operations.
/// </summary>
public interface IUserRepository
{
    Task<User> GetUserByIdAsync(int id);
    Task<int> CreateUserAsync(User user);
}