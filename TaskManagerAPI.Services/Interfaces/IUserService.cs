using TaskManagerAPI.Core.DTOs;

namespace TaskManagerAPI.Services.Interfaces;

public interface IUserService
{
    Task<UserDTO> GetUserByIdAsync(int id);
    Task<int> CreateUserAsync(CreateUserDTO createUserDto);
}