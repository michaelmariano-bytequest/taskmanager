using TaskManagerAPI.DTOs;

namespace TaskManagerAPI.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetUserByIdAsync(int id);
        Task CreateUserAsync(CreateUserDTO createUserDto);
    }
}