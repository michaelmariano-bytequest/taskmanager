using TaskManagerAPI.Core.Enums;

namespace TaskManagerAPI.Core.Entities;

/// <summary>
/// Represents a project within the task manager system.
/// </summary>
public class Project
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public ProjectStatusEnum Status { get; set; }
}