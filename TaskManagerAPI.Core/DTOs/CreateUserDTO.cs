namespace TaskManagerAPI.Core.DTOs;

/// <summary>
/// DTO for creating a new user.
/// </summary>
public class CreateUserDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}