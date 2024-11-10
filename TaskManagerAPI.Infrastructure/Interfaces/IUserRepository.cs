using TaskManagerAPI.Core.Entities;

namespace TaskManagerAPI.Infrastructure.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(int id);
    Task CreateUserAsync(User user);
}