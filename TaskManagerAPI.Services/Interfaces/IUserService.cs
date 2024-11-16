using TaskManagerAPI.Core.DTOs;

namespace TaskManagerAPI.Services.Interfaces;

/// <summary>
/// Provides methods to manage user data within the system.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Asynchronously retrieves a user by their unique ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the UserDTO of the retrieved user.</returns>
    Task<UserDTO> GetUserByIdAsync(int id);

    /// <summary>
    /// Asynchronously creates a new user.
    /// </summary>
    /// <param name="createUserDto">The details of the user to create.</param>
    /// <returns>The ID of the newly created user.</returns>
    Task<int> CreateUserAsync(CreateUserDTO createUserDto);
}