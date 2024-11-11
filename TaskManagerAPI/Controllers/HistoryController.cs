using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Infrastructure.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HistoryController : ControllerBase
{
    private readonly IHistoryService _historyService;
    private readonly IMapper _mapper;

    public HistoryController(IHistoryService historyService, IMapper mapper)
    {
        _historyService = historyService;
        _mapper = mapper;
    }

    /// <summary>
    ///     Retrieve the history of changes for a specific task.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <returns>A list of history records for the specified task.</returns>
    [HttpGet("task/{taskId}")]
    public async Task<ActionResult<List<HistoryDTO>>> GetHistoryByTaskId(int taskId)
    {
        var historyRecords = await _historyService.GetHistoryByTaskIdAsync(taskId);
       
        if (!historyRecords.Any())
            return NotFound();
       
        var historyDTOs = _mapper.Map<List<HistoryDTO>>(historyRecords);
        
        return Ok(historyDTOs);
    }
}