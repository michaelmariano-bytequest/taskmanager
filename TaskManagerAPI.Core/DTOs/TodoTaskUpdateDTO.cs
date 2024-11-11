using TaskManagerAPI.Core.Enums;

namespace TaskManagerAPI.Core.DTOs;

public class TodoTaskUpdateDTO
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DueDate { get; set; }
    public TodoTaskStatusEnum Status { get; set; }
}