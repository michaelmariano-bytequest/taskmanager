using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Controllers;

/// <summary>
/// Controller responsible for handling user-related requests.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    /// <summary>
    /// Service to manage user-related operations.
    /// </summary>
    private readonly IUserService _userService;

    /// <summary>
    /// Controller to handle user-related endpoints.
    /// </summary>
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Retrieve a user by their unique ID.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>The user details.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }

    /// <summary>
    /// Create a new user.
    /// </summary>
    /// <param name="createUserDto">The user details to create.</param>
    /// <returns>The newly created user details.</returns>
    [HttpPost]
    public async Task<ActionResult<int>> CreateUser(CreateUserDTO createUserDto)
    {
        int userId = await _userService.CreateUserAsync(createUserDto);
        return CreatedAtAction(nameof(GetUserById), new { id = userId }, userId);
    }
}