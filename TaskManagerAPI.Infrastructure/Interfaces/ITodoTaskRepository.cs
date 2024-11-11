using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Core.Enums;

namespace TaskManagerAPI.Infrastructure.Interfaces;

public interface ITodoTaskRepository
{
    Task<TodoTask> GetTodoTaskByIdAsync(int id);
    Task<IEnumerable<TodoTask>> GetTasksByProjectIdAsync(int projectId);
    Task<IEnumerable<TodoTask>> GetTasksByProjectIdAndStatusAsync(int projectId, TodoTaskStatusEnum todoTaskStatus);
    Task<int> CreateTodoTaskAsync(TodoTask task);
    Task UpdateTodoTaskAsync(TodoTask task);
    Task DeleteTodoTaskAsync(int id);
}