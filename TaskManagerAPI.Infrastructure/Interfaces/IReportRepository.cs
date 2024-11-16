using TaskManagerAPI.Core.DTOs;


namespace TaskManagerAPI.Infrastructure.Interfaces;

/// <summary>
/// Interface for generating reports related to task completion and user performance.
/// </summary>
public interface IReportRepository
{
    Task<List<UserPerformanceDTO>> GetCompletedTasksReportAsync(
        int? userId = null,
        DateTime? startDate = null,
        DateTime? endDate = null);
}