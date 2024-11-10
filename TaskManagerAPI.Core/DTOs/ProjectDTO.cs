using TaskManagerAPI.Core.Enums;

namespace TaskManagerAPI.Core.DTOs;

public class ProjectDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public ProjectStatusEnum Status { get; set; }
}