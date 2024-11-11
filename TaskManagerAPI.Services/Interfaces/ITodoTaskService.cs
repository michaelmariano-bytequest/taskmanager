using TaskManagerAPI.Core.Common;
using TaskManagerAPI.Core.Entities;

namespace TaskManagerAPI.Services.Interfaces;

public interface ITodoTaskService
{
    Task<TodoTask> GetTodoTaskByIdAsync(int id);
    Task<List<TodoTask>> GetTasksByProjectIdAsync(int projectId);
    Task<Result<TodoTask>> CreateTodoTaskAsync(TodoTask task);
    Task UpdateTodoTaskAsync(TodoTask task);
    Task<Result> SoftDeleteTodoTaskAsync(int taskId);
}