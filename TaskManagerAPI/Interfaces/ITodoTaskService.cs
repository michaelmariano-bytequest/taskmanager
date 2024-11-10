using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Common;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Interfaces
{
    public interface ITodoTaskService
    {
        Task<TodoTask> GetTodoTaskByIdAsync(int id);
        Task<IEnumerable<TodoTask>> GetTasksByProjectIdAsync(int projectId);
        Task<Result<TodoTask>> CreateTodoTaskAsync(TodoTask task);
        Task UpdateTodoTaskAsync(TodoTask task);
        Task<Result> SoftDeleteTodoTaskAsync(int taskId);
    }
}