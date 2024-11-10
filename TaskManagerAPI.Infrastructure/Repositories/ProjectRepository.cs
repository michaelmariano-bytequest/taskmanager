using System.Data;
using Dapper;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Infrastructure.DataAccess;
using TaskManagerAPI.Infrastructure.Interfaces;

namespace TaskManagerAPI.Infrastructure.Repositories;

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

        var parameters = new DynamicParameters();
        parameters.Add("Id", id, DbType.Int32);

        return await _dataAccess.QuerySingleAsync<Project>(sql, parameters);
    }

    public async Task<IEnumerable<Project>> GetProjectsByUserIdAsync(int userId)
    {
        var sql = "SELECT * FROM task_manager.project WHERE user_id = @UserId";

        var parameters = new DynamicParameters();
        parameters.Add("UserId", userId, DbType.Int32);

        return await _dataAccess.QueryAsync<Project>(sql, parameters);
    }

    public async Task<int> CreateProjectAsync(Project project)
    {
        var sql = "INSERT INTO task_manager.project (user_id, name, description, start_date, end_date, status) " +
                  "VALUES (@UserId, @Name, @Description, @StartDate, @EndDate, @Status) RETURNING id;";

        var parameters = new DynamicParameters();
        parameters.Add("UserId", project.UserId, DbType.Int32);
        parameters.Add("Name", project.Name, DbType.String);
        parameters.Add("Description", project.Description, DbType.String);
        parameters.Add("StartDate", project.StartDate, DbType.DateTime);
        parameters.Add("EndDate", project.EndDate, DbType.DateTime);
        parameters.Add("Status", project.Status.ToString(), DbType.String);

        return await _dataAccess.QuerySingleAsync<int>(sql, parameters);
    }

    public async Task UpdateProjectAsync(Project project)
    {
        var sql = "UPDATE task_manager.project SET name = @Name, description = @Description, " +
                  "user_id = @UserId, start_date = @StartDate, end_date = @EndDate, status = @Status WHERE id = @Id";

        var parameters = new DynamicParameters();
        parameters.Add("Id", project.Id, DbType.Int32);
        parameters.Add("Name", project.Name, DbType.String);
        parameters.Add("Description", project.Description, DbType.String);
        parameters.Add("UserId", project.UserId, DbType.Int32);
        parameters.Add("StartDate", project.StartDate, DbType.DateTime);
        parameters.Add("EndDate", project.EndDate, DbType.DateTime);
        parameters.Add("Status", project.Status.ToString(), DbType.String); // Convert enum to string

        await _dataAccess.ExecuteAsync(sql, parameters);
    }

    public async Task DeleteProjectAsync(int id)
    {
        var sql = "DELETE FROM task_manager.project WHERE id = @Id";

        var parameters = new DynamicParameters();
        parameters.Add("Id", id, DbType.Int32);

        await _dataAccess.ExecuteAsync(sql, parameters);
    }
}