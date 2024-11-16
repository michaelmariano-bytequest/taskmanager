using TaskManagerAPI.Core.Common;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Core.Enums;
using TaskManagerAPI.Infrastructure.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services.Services;

/// <summary>
/// Service for managing todo tasks.
/// </summary>
public class TodoTaskService : ITodoTaskService
{
    /// <summary>
    /// Service responsible for managing and retrieving history entries associated with tasks.
    /// Used for logging task creation, updates, deletions, and other historical events for tasks.
    /// </summary>
    private readonly IHistoryService _historyService;

    /// <summary>
    /// Represents a repository for performing CRUD operations on TodoTask entities.
    /// </summary>
    private readonly ITodoTaskRepository _todoTaskRepository;

    /// <summary>
    /// Service class to handle operations related to TodoTask entities.
    /// </summary>
    public TodoTaskService(ITodoTaskRepository todoTaskRepository, IHistoryService historyService)
    {
        _todoTaskRepository = todoTaskRepository;
        _historyService = historyService;
    }

    /// <summary>
    /// Asynchronously retrieves a todo task by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the task to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the todo task with the specified identifier.</returns>
    public async Task<TodoTask> GetTodoTaskByIdAsync(int id)
    {
        return await _todoTaskRepository.GetTodoTaskByIdAsync(id);
    }

    /// <summary>
    /// Retrieves the list of tasks associated with a specific project asynchronously.
    /// </summary>
    /// <param name="projectId">The unique identifier of the project.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of tasks associated with the specified project.</returns>
    public async Task<List<TodoTask>> GetTasksByProjectIdAsync(int projectId)
    {
        return await _todoTaskRepository.GetTasksByProjectIdAsync(projectId);
    }

    /// <summary>
    /// Creates a new TodoTask asynchronously, adds a history entry, and returns the result.
    /// </summary>
    /// <param name="task">The TodoTask object to be created.</param>
    /// <returns>A Result object containing the created TodoTask if successful; otherwise, a failure message.</returns>
    public async Task<Result<TodoTask>> CreateTodoTaskAsync(TodoTask task)
    {
        var tasksInProject = await _todoTaskRepository.GetTasksByProjectIdAsync(task.ProjectId);

        if (tasksInProject.Count() >= 20)
            return Result<TodoTask>.Failure("The project has reached the maximum limit of 20 tasks.");

        int id = await _todoTaskRepository.CreateTodoTaskAsync(task);
        task.Id = id;
        await _historyService.AddHistoryEntryAsync(task.Id, "Task created", task);
        
        return Result<TodoTask>.Success(task);
    }

    /// <summary>
    /// Updates an existing TodoTask asynchronously.
    /// </summary>
    /// <param name="task">The TodoTask object that needs to be updated.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task UpdateTodoTaskAsync(TodoTask task)
    {
        await _todoTaskRepository.UpdateTodoTaskAsync(task);
        await _historyService.AddHistoryEntryAsync(task.Id, "Task updated.", task);
    }

    /// <summary>
    /// Marks a Todo task as deleted.
    /// </summary>
    /// <param name="taskId">The ID of the task to be deleted.</param>
    /// <returns>A result indicating success or failure.</returns>
    public async Task<Result> SoftDeleteTodoTaskAsync(int taskId)
    {
        // Obtenha a tarefa para verificar se ela existe
        var task = await _todoTaskRepository.GetTodoTaskByIdAsync(taskId);

        if (task == null) return Result.Failure("Task not found.");

        // Marcar a tarefa como 'Deleted'
        task.Status = TodoTaskStatusEnum.Deleted;
        await _todoTaskRepository.UpdateTodoTaskAsync(task);

        // Adicionar uma entrada ao histórico informando a deleção
        await _historyService.AddHistoryEntryAsync(task.Id, "Task marked as deleted", task);

        return Result.Success();
    }
}