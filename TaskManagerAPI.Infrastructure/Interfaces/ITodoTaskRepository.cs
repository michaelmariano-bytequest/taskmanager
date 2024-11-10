using TaskManagerAPI.Core.Entities;

namespace TaskManagerAPI.Infrastructure.Interfaces;

public interface ITodoTaskRepository
{
    Task<TodoTask> GetTodoTaskByIdAsync(int id);
    Task<IEnumerable<TodoTask>> GetTasksByProjectIdAsync(int projectId);
    Task<int> CreateTodoTaskAsync(TodoTask task);
    Task UpdateTodoTaskAsync(TodoTask task);
    Task DeleteTodoTaskAsync(int id);
}