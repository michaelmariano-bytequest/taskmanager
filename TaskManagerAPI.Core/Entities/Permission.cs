namespace TaskManagerAPI.Core.Entities;

/// <summary>
/// Represents a Permission entity within the TaskManagerAPI.Core.Entities namespace.
/// </summary>
public class Permission
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}