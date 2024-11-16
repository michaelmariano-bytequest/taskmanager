using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Infrastructure.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services.Services
{
    /// <summary>
    /// Service for generating various types of reports within the Task Manager API.
    /// </summary>
    public class ReportService : IReportService
    {
        /// <summary>
        /// Repository instance for handling storage and retrieval of report data.
        /// </summary>
        private readonly IReportRepository _reportRepository;

        /// <summary>
        /// Service class responsible for generating performance reports.
        /// </summary>
        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        /// <summary>
        /// Asynchronously generates a performance report for a given user within the specified date range.
        /// </summary>
        /// <param name="userId">The ID of the user to generate the report for. If null, the report will include all users.</param>
        /// <param name="startDate">The start date of the date range for the report. If null, the report will start from the earliest available date.</param>
        /// <param name="endDate">The end date of the date range for the report. If null, the report will end at the latest available date.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="PerformanceReportDTO"/> with the performance data.</returns>
        public async Task<PerformanceReportDTO> GeneratePerformanceReportAsync(int? userId, DateTime? startDate, DateTime? endDate)
        {
            var reportData = await _reportRepository.GetCompletedTasksReportAsync(userId, startDate, endDate);
            return new PerformanceReportDTO { Data = reportData };
        }
    }
}