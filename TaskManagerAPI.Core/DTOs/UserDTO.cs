namespace TaskManagerAPI.Core.DTOs;

/// <summary>
/// Data Transfer Object representing a user.
/// </summary>
public class UserDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}