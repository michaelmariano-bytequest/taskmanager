namespace TaskManagerAPI.Core.Entities;

public class History
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public string Description { get; set; }
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
}