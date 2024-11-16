using TaskManagerAPI.Core.DTOs;

namespace TaskManagerAPI.Services.Interfaces;

/// <summary>
/// Interface for generating performance reports within the Task Manager API.
/// </summary>
public interface IReportService
{
    /// <summary>
    /// Generates a performance report based on completed tasks within a specified date range.
    /// The report can be specific to a user or cover multiple users depending on the specified userId.
    /// </summary>
    /// <param name="userId">Optional user ID to filter the performance report for a specific user. If null, report includes all users.</param>
    /// <param name="startDate">Optional start date to filter the performance report. If null, no start date filter is applied.</param>
    /// <param name="endDate">Optional end date to filter the performance report. If null, no end date filter is applied.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the performance report data encapsulated in a PerformanceReportDTO.</returns>
    Task<PerformanceReportDTO> GeneratePerformanceReportAsync(int? userId, DateTime? startDate, DateTime? endDate);
}