using TaskManagerAPI.DataAccess;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Interfaces;

namespace TaskManagerAPI.Repositories
{
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
            return await _dataAccess.QueryAsync<History>(sql, new { TaskId = taskId });
        }

        public async Task AddHistoryAsync(History history)
        {
            var sql = "INSERT INTO task_manager.history (task_id, description, modified_at) " +
                      "VALUES (@TaskId, @Description, @ModifiedAt)";
            await _dataAccess.ExecuteAsync(sql, history);
        }
    }
}