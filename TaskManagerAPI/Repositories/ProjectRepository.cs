using TaskManagerAPI.DataAccess;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Interfaces;

namespace TaskManagerAPI.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ISqlDataAccess _dataAccess;

        public ProjectRepository(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            var sql = "SELECT * FROM task_manager.project WHERE id = @Id";
            return await _dataAccess.QuerySingleAsync<Project>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Project>> GetProjectsByUserIdAsync(int userId)
        {
            var sql = "SELECT * FROM task_manager.project WHERE user_id = @UserId";
            return await _dataAccess.QueryAsync<Project>(sql, new { UserId = userId });
        }

        public async Task CreateProjectAsync(Project project)
        {
            var sql = "INSERT INTO task_manager.project (user_id, name, description, start_date, end_date, status) " +
                      "VALUES (@UserId, @Name, @Description, @StartDate, @EndDate, @Status)";
            await _dataAccess.ExecuteAsync(sql, project);
        }

        public async Task UpdateProjectAsync(Project project)
        {
            var sql = "UPDATE task_manager.project SET name = @Name, description = @Description, " +
                      "start_date = @StartDate, end_date = @EndDate, status = @Status WHERE id = @Id";
            await _dataAccess.ExecuteAsync(sql, project);
        }

        public async Task DeleteProjectAsync(int id)
        {
            var sql = "DELETE FROM task_manager.project WHERE id = @Id";
            await _dataAccess.ExecuteAsync(sql, new { Id = id });
        }
    }
}