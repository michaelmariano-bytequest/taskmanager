using System.Data;
using Dapper;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Infrastructure.DataAccess;
using TaskManagerAPI.Infrastructure.Interfaces;

namespace TaskManagerAPI.Infrastructure.Repositories;

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

        var parameters = new DynamicParameters();
        parameters.Add("Id", id, DbType.Int32);

        return await _dataAccess.QuerySingleAsync<TodoTask>(sql, parameters);
    }

    public async Task<IEnumerable<TodoTask>> GetTasksByProjectIdAsync(int projectId)
    {
        var sql = "SELECT * FROM task_manager.todo_task WHERE project_id = @ProjectId";

        var parameters = new DynamicParameters();
        parameters.Add("ProjectId", projectId, DbType.Int32);

        return await _dataAccess.QueryAsync<TodoTask>(sql, parameters);
    }

    public async Task<int> CreateTodoTaskAsync(TodoTask task)
    {
        var sql = "INSERT INTO task_manager.todo_task (project_id, title, description, created_at, due_date, priority, status) " +
                  "VALUES (@ProjectId, @Title, @Description, @CreatedAt, @DueDate, @Priority, @Status) RETURNING id;";

        var parameters = new DynamicParameters();
        parameters.Add("ProjectId", task.ProjectId, DbType.Int32);
        parameters.Add("Title", task.Title, DbType.String);
        parameters.Add("Description", task.Description, DbType.String);
        parameters.Add("CreatedAt", task.CreatedAt, DbType.DateTime);
        parameters.Add("DueDate", task.DueDate, DbType.DateTime);
        parameters.Add("Priority", task.Priority.ToString(), DbType.String); // Convert enum to string
        parameters.Add("Status", task.Status.ToString(), DbType.String); // Convert enum to string

        return await _dataAccess.QuerySingleAsync<int>(sql, parameters);
    }

    public async Task UpdateTodoTaskAsync(TodoTask task)
    {
        var sql = "UPDATE task_manager.todo_task SET title = @Title, description = @Description, " +
                  "due_date = @DueDate, priority = @Priority, status = @Status WHERE id = @Id";

        var parameters = new DynamicParameters();
        parameters.Add("Id", task.Id, DbType.Int32);
        parameters.Add("Title", task.Title, DbType.String);
        parameters.Add("Description", task.Description, DbType.String);
        parameters.Add("DueDate", task.DueDate, DbType.DateTime);
        parameters.Add("Priority", task.Priority.ToString(), DbType.String); // Convert enum to string
        parameters.Add("Status", task.Status.ToString(), DbType.String); // Convert enum to string

        await _dataAccess.ExecuteAsync(sql, parameters);
    }

    public async Task DeleteTodoTaskAsync(int id)
    {
        var sql = "DELETE FROM task_manager.todo_task WHERE id = @Id";

        var parameters = new DynamicParameters();
        parameters.Add("Id", id, DbType.Int32);

        await _dataAccess.ExecuteAsync(sql, parameters);
    }
}