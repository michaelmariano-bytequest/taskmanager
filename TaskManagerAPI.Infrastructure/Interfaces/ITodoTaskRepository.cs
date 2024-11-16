using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Core.Enums;

namespace TaskManagerAPI.Infrastructure.Interfaces;

/// <summary>
/// Provides an interface for CRUD operations on TodoTask entities.
/// </summary>
public interface ITodoTaskRepository
{
    Task<TodoTask> GetTodoTaskByIdAsync(int id);
    Task<List<TodoTask>> GetTasksByProjectIdAsync(int projectId);
    Task<List<TodoTask>> GetTasksByProjectIdAndStatusAsync(int projectId, TodoTaskStatusEnum todoTaskStatus);
    Task<int> CreateTodoTaskAsync(TodoTask task);
    Task UpdateTodoTaskAsync(TodoTask task);
    Task DeleteTodoTaskAsync(int id);
}