using TaskManagerAPI.Core.DTOs;


namespace TaskManagerAPI.Infrastructure.Interfaces;

public interface IReportRepository
{
    Task<IEnumerable<UserPerformanceDTO>> GetCompletedTasksReportAsync();
}