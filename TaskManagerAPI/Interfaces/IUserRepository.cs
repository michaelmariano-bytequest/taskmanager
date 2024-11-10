using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task CreateUserAsync(User user);
    }
}