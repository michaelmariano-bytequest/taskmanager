using System.Data;
using Dapper;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Infrastructure.DataAccess;
using TaskManagerAPI.Infrastructure.Interfaces;

namespace TaskManagerAPI.Infrastructure.Repositories;

public class HistoryRepository : IHistoryRepository
{
    private readonly ISqlDataAccess _dataAccess;

    public HistoryRepository(ISqlDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<IEnumerable<History>> GetHistoryByTaskIdAsync(int taskId)
    {
        var sql = "SELECT * FROM task_manager.history WHERE task_id = @TaskId ORDER BY modified_at DESC";

        var parameters = new DynamicParameters();
        parameters.Add("TaskId", taskId, DbType.Int32);

        return await _dataAccess.QueryAsync<History>(sql, parameters);
    }

    public async Task AddHistoryAsync(History history)
    {
        var sql = "INSERT INTO task_manager.history (task_id, description, modified_at, userid) " +
                  "VALUES (@TaskId, @Description, @ModifiedAt, @UserId)";

        var parameters = new DynamicParameters();
        parameters.Add("TaskId", history.TaskId, DbType.Int32);
        parameters.Add("Description", history.Description, DbType.String);
        parameters.Add("ModifiedAt", history.ModifiedAt, DbType.DateTime);
        parameters.Add("UserId", history.UserId, DbType.Int32);

        await _dataAccess.ExecuteAsync(sql, parameters);
    }
}