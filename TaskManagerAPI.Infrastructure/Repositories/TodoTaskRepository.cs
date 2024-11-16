using System.Data;
using Dapper;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Core.Enums;
using TaskManagerAPI.Infrastructure.DataAccess;
using TaskManagerAPI.Infrastructure.Interfaces;

namespace TaskManagerAPI.Infrastructure.Repositories;

/// <summary>
/// Repository class for managing TodoTask entities.
/// </summary>
public class TodoTaskRepository : ITodoTaskRepository
{
    /// <summary>
    /// Provides access to SQL data operations.
    /// Used for executing queries and commands against the database.
    /// </summary>
    private readonly ISqlDataAccess _dataAccess;

    /// <summary>
    /// Repository for managing TodoTasks in the Task Manager application.
    /// This class provides methods for CRUD operations and querying tasks based on specific criteria.
    /// Implements the ITodoTaskRepository interface.
    /// </summary>
    public TodoTaskRepository(ISqlDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    /// <summary>
    /// Asynchronously retrieves a TodoTask by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the TodoTask.</param>
    /// <returns>A task representing the asynchronous operation, with a <see cref="TodoTask"/> object as the result.</returns>
    public async Task<TodoTask> GetTodoTaskByIdAsync(int id)
    {
        var sql = "SELECT * FROM task_manager.todo_task WHERE id = @Id and status <> 'Deleted';";

        var parameters = new DynamicParameters();
        parameters.Add("Id", id, DbType.Int32);

        return await _dataAccess.QuerySingleAsync<TodoTask>(sql, parameters);
    }

    /// <summary>
    /// Asynchronously retrieves a list of TodoTask instances associated with a specified project ID.
    /// </summary>
    /// <param name="projectId">The ID of the project for which to retrieve tasks.</param>
    /// <returns>A Task that represents the asynchronous operation. The task result contains a list of TodoTask objects associated with the specified project ID.</returns>
    public async Task<List<TodoTask>> GetTasksByProjectIdAsync(int projectId)
    {
        var sql = "SELECT * FROM task_manager.todo_task WHERE projectid = @ProjectId and status <> 'Deleted';";

        var parameters = new DynamicParameters();
        parameters.Add("ProjectId", projectId, DbType.Int32);

        return await _dataAccess.QueryAsync<TodoTask>(sql, parameters);
    }

    /// <summary>
    /// Retrieves a list of TodoTask entities based on the provided project ID and task status.
    /// </summary>
    /// <param name="projectId">The ID of the project for which tasks are to be retrieved.</param>
    /// <param name="todoTaskStatus">The status of the tasks to be retrieved.</param>
    /// <returns>A Task representing the asynchronous operation, containing a list of TodoTask objects that match the specified project ID and task status.</returns>
    public async Task<List<TodoTask>> GetTasksByProjectIdAndStatusAsync(int projectId, TodoTaskStatusEnum todoTaskStatus)
    {
        var sql = "SELECT * FROM task_manager.todo_task WHERE projectid = @ProjectId AND status = @TodoTaskStatus;";

        var parameters = new DynamicParameters();
        parameters.Add("ProjectId", projectId, DbType.Int32);
        parameters.Add("TodoTaskStatus", todoTaskStatus.ToString(), DbType.String); // Converte enum para string

        return await _dataAccess.QueryAsync<TodoTask>(sql, parameters);
    }

    /// <summary>
    /// Asynchronously creates a new TodoTask in the database and returns the generated identifier.
    /// </summary>
    /// <param name="task">The TodoTask entity containing details of the task to be created.</param>
    /// <returns>The identifier of the newly created TodoTask.</returns>
    public async Task<int> CreateTodoTaskAsync(TodoTask task)
    {
        var sql = "INSERT INTO task_manager.todo_task (projectid, title, description, createdat, duedate, priority, status) " +
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

    /// <summary>
    /// Updates a TodoTask in the database asynchronously.
    /// </summary>
    /// <param name="task">The TodoTask entity to be updated. The task must have a valid Id, Title, Description, DueDate, and Status.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task UpdateTodoTaskAsync(TodoTask task)
    {
        var sql = "UPDATE task_manager.todo_task SET title = @Title, description = @Description, " +
                  "duedate = @DueDate, status = @Status WHERE id = @Id;";

        var parameters = new DynamicParameters();
        parameters.Add("Id", task.Id, DbType.Int32);
        parameters.Add("Title", task.Title, DbType.String);
        parameters.Add("Description", task.Description, DbType.String);
        parameters.Add("DueDate", task.DueDate, DbType.DateTime2);
        parameters.Add("Status", task.Status.ToString(), DbType.String);

        await _dataAccess.ExecuteAsync(sql, parameters);
    }

    /// <summary>
    /// Deletes a todo task from the database using the specified ID.
    /// </summary>
    /// <param name="id">The unique identifier of the todo task to be deleted.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    public async Task DeleteTodoTaskAsync(int id)
    {
        var sql = "DELETE FROM task_manager.todo_task WHERE id = @Id;";

        var parameters = new DynamicParameters();
        parameters.Add("Id", id, DbType.Int32);

        await _dataAccess.ExecuteAsync(sql, parameters);
    }
}