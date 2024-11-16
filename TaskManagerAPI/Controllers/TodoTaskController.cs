using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Core.Common;
using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Controllers;

/// <summary>
/// Controller for managing todo tasks.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TodoTaskController : ControllerBase
{
    /// <summary>
    /// Service interface for handling ToDo task related operations.
    /// </summary>
    private readonly ITodoTaskService _todoTaskService;

    /// <summary>
    /// Handles object mapping between different layers and representations within the application.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Controller for handling ToDo task-related operations.
    /// </summary>
    public TodoTaskController(ITodoTaskService todoTaskService, IMapper mapper)
    {
        _todoTaskService = todoTaskService;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieve all tasks by projectId.
    /// </summary>
    /// <param name="projectId">The projectId of the related project.</param>
    /// <returns>The task details.</returns>
    [HttpGet("projects/{projectId}/tasks")]
    public async Task<ActionResult<List<TodoTaskCreateDTO>>> GetTasksByProjectId(int projectId)
    {
        var tasks = await _todoTaskService.GetTasksByProjectIdAsync(projectId);

        if (!tasks.Any())
            return NotFound();

        return Ok(tasks);
    }

    /// <summary>
    /// Retrieve a task by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <returns>The task details.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoTaskCreateDTO>> GetTodoTaskById(int id)
    {
        var task = await _todoTaskService.GetTodoTaskByIdAsync(id);
        
        if (task == null)
            return NotFound();
        
        return Ok(task);
    }

    /// <summary>
    /// Create a new task.
    /// </summary>
    /// <param name="task">The details of the task to create.</param>
    /// <returns>The newly created task details.</returns>
    [HttpPost]
    public async Task<ActionResult> CreateTodoTask(TodoTask task)
    {
        var result = await _todoTaskService.CreateTodoTaskAsync(task);

        if (!result.IsSuccess)
        {
            var errorResponse = new ErrorResponse
            {
                Message = result.ErrorMessage
            };
            return BadRequest(errorResponse);
        }

        return CreatedAtAction(nameof(GetTodoTaskById), new { id = result.Value.Id }, result.Value);
    }

    /// <summary>
    /// Update an existing task. The task priority parameter can't be updated.
    /// </summary>
    /// <param name="id">The ID of the task to update.</param>
    /// <param name="taskCreate">The updated task details.</param>
    /// <returns>A no content response indicating the task was updated or a bad request if the IDs do not match.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTodoTask(int id, TodoTaskUpdateDTO taskCreate)
    {
        if (id != taskCreate.Id)
            return BadRequest();

        var todoTask = _mapper.Map<TodoTask>(taskCreate);
        await _todoTaskService.UpdateTodoTaskAsync(todoTask);

        return NoContent();
    }

    /// <summary>
    /// Delete a task by ID.
    /// </summary>
    /// <param name="id">The ID of the task to delete.</param>
    /// <returns>A status indicating the result of the delete operation.</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTodoTask(int id)
    {
        var result = await _todoTaskService.SoftDeleteTodoTaskAsync(id);

        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);

        return NoContent();
    }
}