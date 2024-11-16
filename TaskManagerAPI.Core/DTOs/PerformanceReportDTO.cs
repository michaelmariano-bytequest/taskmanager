namespace TaskManagerAPI.Core.DTOs
{
    /// <summary>
    /// Data Transfer Object for encapsulating the performance report.
    /// </summary>
    public class PerformanceReportDTO
    {
        public List<UserPerformanceDTO> Data { get; set; }
    }
}
