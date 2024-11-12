using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Infrastructure.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<PerformanceReportDTO> GeneratePerformanceReportAsync(int? userId, DateTime? startDate, DateTime? endDate)
        {
            var reportData = await _reportRepository.GetCompletedTasksReportAsync(userId, startDate, endDate);
            return new PerformanceReportDTO { Data = reportData };
        }
    }
}