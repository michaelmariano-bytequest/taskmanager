namespace TaskManagerAPI.Core.DTOs;

/// <summary>
/// Data Transfer Object for encapsulating the performance metrics of a user.
/// </summary>
public class UserPerformanceDTO
{
    public int UserId { get; set; }
    public double AvgCompletedTasksPerDay { get; set; }
}