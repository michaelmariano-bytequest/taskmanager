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
    /// <returns>The performance report data.</returns>
    [HttpGet("performance")]
    public async Task<ActionResult> GetPerformanceReport([FromQuery] string role)
    {
        if (role?.ToLower() != "manager")
        {
            return Unauthorized("Only users with the 'manager' role can access this report.");
        }

        var report = await _reportService.GeneratePerformanceReportAsync();
        return Ok(report);
    }
}
