using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Interfaces
{
    public interface ITodoTaskRepository
    {
        Task<TodoTask> GetTodoTaskByIdAsync(int id);
        Task<IEnumerable<TodoTask>> GetTasksByProjectIdAsync(int projectId);
        Task CreateTodoTaskAsync(TodoTask task);
        Task UpdateTodoTaskAsync(TodoTask task);
        Task DeleteTodoTaskAsync(int id);
    }
}