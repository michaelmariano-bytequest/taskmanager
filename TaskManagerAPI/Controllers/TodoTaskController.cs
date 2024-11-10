using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Interfaces;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoTaskController : ControllerBase
    {
        private readonly ITodoTaskService _todoTaskService;

        public TodoTaskController(ITodoTaskService todoTaskService)
        {
            _todoTaskService = todoTaskService;
        }

        /// <summary>
        /// Retrieve a task by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the task.</param>
        /// <returns>The task details.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoTaskDTO>> GetTodoTaskById(int id)
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
        public async Task<IActionResult> CreateTodoTask(TodoTask task)
        {
            var result = await _todoTaskService.CreateTodoTaskAsync(task);

            if (!result.IsSuccess)
            {
                return BadRequest(new { ErrorCode = "TaskLimitExceeded", Message = result.ErrorMessage });
            }

            return CreatedAtAction(nameof(GetTodoTaskById), new { id = result.Value.Id }, result.Value);
        }

        /// <summary>
        /// Update an existing task.
        /// </summary>
        /// <param name="id">The ID of the task to update.</param>
        /// <param name="task">The updated task details.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoTask(int id, TodoTask task)
        {
            if (id != task.Id)
                return BadRequest();
            await _todoTaskService.UpdateTodoTaskAsync(task);
            return NoContent();
        }

        /// <summary>
        /// Delete a task by ID.
        /// </summary>
        /// <param name="id">The ID of the task to delete.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoTask(int id)
        {
            var result = await _todoTaskService.SoftDeleteTodoTaskAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound(new { Message = result.ErrorMessage });
            }

            return NoContent();
        }
    }
}