using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// Generate performance report for managers.
    /// </summary>
    /// <param name="role">The role of the user requesting the report.</param>
    /// <param name="userId">Use the userId if you want to view performance for a user
    /// else you view all team performance</param>
    /// <param name="startdate">Date to start the search.</param>
    /// <param name="enddate">Date to end the search</param>
    /// <returns>The performance report data.</returns>
    [HttpGet("performance")]
    public async Task<ActionResult> GetPerformanceReport([FromQuery] int? userId, [FromQuery] DateTime? startdate,
        [FromQuery] DateTime? enddate, [FromQuery] string role)
    {
        if (role?.ToLower() != "manager")
        {
            return Unauthorized("Only users with the 'manager' role can access this report.");
        }

        var report = await _reportService.GeneratePerformanceReportAsync(userId, startdate, enddate);

        return Ok(report);
    }
}