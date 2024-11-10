using TaskManagerAPI.DataAccess;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Interfaces;

namespace TaskManagerAPI.Repositories
{
    public class TodoTaskRepository : ITodoTaskRepository
    {
        private readonly ISqlDataAccess _dataAccess;

        public TodoTaskRepository(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<TodoTask> GetTodoTaskByIdAsync(int id)
        {
            var sql = "SELECT * FROM task_manager.todo_task WHERE id = @Id";
            return await _dataAccess.QuerySingleAsync<TodoTask>(sql, new { Id = id });
        }

        public async Task<IEnumerable<TodoTask>> GetTasksByProjectIdAsync(int projectId)
        {
            var sql = "SELECT * FROM task_manager.todo_task WHERE project_id = @ProjectId";
            return await _dataAccess.QueryAsync<TodoTask>(sql, new { ProjectId = projectId });
        }

        public async Task CreateTodoTaskAsync(TodoTask task)
        {
            var sql = "INSERT INTO task_manager.todo_task (project_id, title, description, created_at, due_date, priority, status) " +
                      "VALUES (@ProjectId, @Title, @Description, @CreatedAt, @DueDate, @Priority, @Status)";
            await _dataAccess.ExecuteAsync(sql, task);
        }

        public async Task UpdateTodoTaskAsync(TodoTask task)
        {
            var sql = "UPDATE task_manager.todo_task SET title = @Title, description = @Description, " +
                      "due_date = @DueDate, priority = @Priority, status = @Status WHERE id = @Id";
            await _dataAccess.ExecuteAsync(sql, task);
        }

        public async Task DeleteTodoTaskAsync(int id)
        {
            var sql = "DELETE FROM task_manager.todo_task WHERE id = @Id";
            await _dataAccess.ExecuteAsync(sql, new { Id = id });
        }
    }
}