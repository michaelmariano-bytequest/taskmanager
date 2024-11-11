using TaskManagerAPI.Core.DTOs;


namespace TaskManagerAPI.Infrastructure.Interfaces;

public interface IReportRepository
{
    Task<List<UserPerformanceDTO>> GetCompletedTasksReportAsync(
        int? userId = null,
        DateTime? startDate = null,
        DateTime? endDate = null);
}