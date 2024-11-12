using TaskManagerAPI.Core.DTOs;

namespace TaskManagerAPI.Services.Interfaces;

public interface IReportService
{
    Task<PerformanceReportDTO> GeneratePerformanceReportAsync(int? userId, DateTime? startDate, DateTime? endDate);
}