using System.Data;
using Dapper;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Infrastructure.DataAccess;
using TaskManagerAPI.Infrastructure.Interfaces;

namespace TaskManagerAPI.Infrastructure.Repositories;

/// <summary>
/// Provides methods for accessing and managing task history records in the repository.
/// </summary>
public class HistoryRepository : IHistoryRepository
{
    /// <summary>
    /// Provides access to SQL data operations for the repository.
    /// </summary>
    private readonly ISqlDataAccess _dataAccess;

    /// <summary>
    /// Repository for accessing and managing task history records.
    /// </summary>
    public HistoryRepository(ISqlDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    /// <summary>
    /// Retrieves the history associated with a specific task by its task ID.
    /// </summary>
    /// <param name="taskId">The ID of the task for which history records are to be retrieved.</param>
    /// <returns>A task representing an asynchronous operation that returns a list of history records.</returns>
    public async Task<List<History>> GetHistoryByTaskIdAsync(int taskId)
    {
        var sql = "SELECT * FROM task_manager.history WHERE taskid = @TaskId ORDER BY modifiedat DESC";

        var parameters = new DynamicParameters();
        parameters.Add("TaskId", taskId, DbType.Int32);

        return await _dataAccess.QueryAsync<History>(sql, parameters);
    }

    /// <summary>
    /// Asynchronously adds a history record to the database.
    /// </summary>
    /// <param name="history">The history record to be added. This includes details such as task ID, description, modification date, and user ID.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task AddHistoryAsync(History history)
    {
        var sql = "INSERT INTO task_manager.history (taskid, description, modifiedat, userid) " +
                  "VALUES (@TaskId, @Description, @ModifiedAt, @UserId)";

        var parameters = new DynamicParameters();
        parameters.Add("TaskId", history.TaskId, DbType.Int32);
        parameters.Add("Description", history.Description, DbType.String);
        parameters.Add("ModifiedAt", history.ModifiedAt, DbType.DateTime);
        parameters.Add("UserId", history.UserId, DbType.Int32);

        await _dataAccess.ExecuteAsync(sql, parameters);
    }
}