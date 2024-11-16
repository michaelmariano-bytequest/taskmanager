using TaskManagerAPI.Core.Common;
using TaskManagerAPI.Core.Entities;

namespace TaskManagerAPI.Services.Interfaces;

/// <summary>
/// Interface defining the contract for ToDo task service operations.
/// </summary>
public interface ITodoTaskService
{
    /// <summary>
    /// Asynchronously retrieves a to-do task by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the to-do task.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the to-do task if found; otherwise, null.</returns>
    Task<TodoTask> GetTodoTaskByIdAsync(int id);

    /// <summary>
    /// Asynchronously retrieves a list of tasks associated with a specific project ID.
    /// </summary>
    /// <param name="projectId">The ID of the project for which to retrieve tasks.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of TodoTask objects associated with the specified project ID.</returns>
    Task<List<TodoTask>> GetTasksByProjectIdAsync(int projectId);

    /// <summary>
    /// Creates a new to-do task.
    /// </summary>
    /// <param name="task">The task to be created, containing details such as title, description, project ID, priority, and due date.</param>
    /// <returns>
    /// A Result object containing the created TodoTask if the operation is successful,
    /// or an error message if the project has reached its maximum limit of tasks or another error occurs.
    /// </returns>
    Task<Result<TodoTask>> CreateTodoTaskAsync(TodoTask task);

    /// <summary>
    /// Updates an existing to-do task.
    /// </summary>
    /// <param name="task">The task to be updated, with the new values.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateTodoTaskAsync(TodoTask task);

    /// <summary>
    /// Soft deletes a ToDo task by marking its status as 'Deleted' instead of removing it from the database.
    /// </summary>
    /// <param name="taskId">The ID of the task to be soft deleted.</param>
    /// <returns>A Result indicating the success or failure of the operation.</returns>
    Task<Result> SoftDeleteTodoTaskAsync(int taskId);
}