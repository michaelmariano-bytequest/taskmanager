using AutoMapper;
using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Infrastructure.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services.Services;

/// <summary>
/// Implements the <see cref="IUserService"/> interface to provide user-related operations.
/// </summary>
public class UserService : IUserService
{
    /// <summary>
    /// Instance of IMapper used for mapping objects between different data models.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Repository for accessing user data storage, used in various user-related operations.
    /// </summary>
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Service for managing user-related operations.
    /// </summary>
    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves a user by their unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>A Task representing the asynchronous operation, with a UserDTO result containing the user's information.</returns>
    public async Task<UserDTO> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        return _mapper.Map<UserDTO>(user);
    }

    /// <summary>
    /// Asynchronously creates a new user in the system.
    /// </summary>
    /// <param name="createUserDto">The data transfer object containing details for the new user.</param>
    /// <returns>The unique identifier of the created user.</returns>
    public async Task<int> CreateUserAsync(CreateUserDTO createUserDto)
    {
        var user = _mapper.Map<User>(createUserDto);
        user.PasswordHash = HashPassword(createUserDto.Password);
        
        return await _userRepository.CreateUserAsync(user);
    }

    /// <summary>
    /// Hashes a given password using the BCrypt hashing algorithm.
    /// </summary>
    /// <param name="password">The plain-text password to hash.</param>
    /// <returns>A string representing the hashed password.</returns>
    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// Verifies a given password against a stored hashed password.
    /// </summary>
    /// <param name="password">The plain text password to verify.</param>
    /// <param name="hashedPassword">The hashed password to compare against.</param>
    /// <returns>True if the password matches the hashed password; otherwise, false.</returns>
    private bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}