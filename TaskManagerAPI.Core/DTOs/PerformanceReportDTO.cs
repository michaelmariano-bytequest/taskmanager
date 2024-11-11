namespace TaskManagerAPI.Core.DTOs
{
    public class PerformanceReportDTO
    {
        public List<UserPerformanceDTO> Data { get; set; }
    }

    public class UserPerformanceDTO
    {
        public int UserId { get; set; }
        public double AvgCompletedTasksPerDay { get; set; }
    }
}
