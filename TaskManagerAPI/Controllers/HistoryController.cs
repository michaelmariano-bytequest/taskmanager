using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Infrastructure.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Controllers;

/// <summary>
/// Controller responsible for managing the history of changes to tasks.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HistoryController : ControllerBase
{
    /// <summary>
    /// Provides history-related operations and interactions for tasks.
    /// </summary>
    private readonly IHistoryService _historyService;

    /// <summary>
    /// Injected instance of the AutoMapper IMapper interface used for mapping entities
    /// to Data Transfer Objects (DTOs) and vice versa. The _mapper variable simplifies
    /// the transformation of data structures within the controller, ensuring consistency
    /// and reducing boilerplate code by leveraging the mapping configurations defined in AutoMapper.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Controller for handling history-related operations.
    /// </summary>
    public HistoryController(IHistoryService historyService, IMapper mapper)
    {
        _historyService = historyService;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieve the history of changes for a specific task.
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