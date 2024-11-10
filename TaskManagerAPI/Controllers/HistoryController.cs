using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Infrastructure.Interfaces;

namespace TaskManagerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HistoryController : ControllerBase
{
    private readonly IHistoryRepository _historyRepository;
    private readonly IMapper _mapper;

    public HistoryController(IHistoryRepository historyRepository, IMapper mapper)
    {
        _historyRepository = historyRepository;
        _mapper = mapper;
    }

    /// <summary>
    ///     Retrieve the history of changes for a specific task.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <returns>A list of history records for the specified task.</returns>
    [HttpGet("task/{taskId}")]
    public async Task<ActionResult<IEnumerable<HistoryDTO>>> GetHistoryByTaskId(int taskId)
    {
        var historyRecords = await _historyRepository.GetHistoryByTaskIdAsync(taskId);
        if (!historyRecords.Any())
            return NotFound();
        var historyDTOs = _mapper.Map<IEnumerable<HistoryDTO>>(historyRecords);
        return Ok(historyDTOs);
    }
}