using System.Data;
using Dapper;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Infrastructure.DataAccess;
using TaskManagerAPI.Infrastructure.Interfaces;

namespace TaskManagerAPI.Infrastructure.Repositories;

/// <summary>
/// Repository for performing CRUD operations on Project entities.
/// </summary>
public class ProjectRepository : IProjectRepository
{
    /// <summary>
    /// Provides data access functionality for executing SQL queries and commands.
    /// </summary>
    private readonly ISqlDataAccess _dataAccess;

    /// <summary>
    /// A repository class for managing project data in the database.
    /// Implements the <see cref="IProjectRepository"/> interface.
    /// </summary>
    public ProjectRepository(ISqlDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    /// <summary>
    /// Retrieves a project by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the project.</param>
    /// <returns>A task representing the asynchronous operation that returns the project entity.</returns>
    public async Task<Project> GetProjectByIdAsync(int id)
    {
        var sql = "SELECT * FROM task_manager.project WHERE id = @Id and status <> 'Deleted';";

        var parameters = new DynamicParameters();
        parameters.Add("Id", id, DbType.Int32);

        return await _dataAccess.QuerySingleAsync<Project>(sql, parameters);
    }

    /// <summary>
    /// Retrieves a list of projects associated with a specific user that are not marked as 'Deleted'.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose projects are to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of projects related to the specified user.</returns>
    public async Task<List<Project>> GetProjectsByUserIdAsync(int userId)
    {
        var sql = "SELECT * FROM task_manager.project WHERE userid = @UserId and status <> 'Deleted';";

        var parameters = new DynamicParameters();
        parameters.Add("UserId", userId, DbType.Int32);

        return await _dataAccess.QueryAsync<Project>(sql, parameters);
    }

    /// <summary>
    /// Creates a new project asynchronously and returns its ID.
    /// </summary>
    /// <param name="project">The project entity containing details of the project to be created.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the ID of the newly created project.</returns>
    public async Task<int> CreateProjectAsync(Project project)
    {
        var sql = "INSERT INTO task_manager.project (userid, name, description, startdate, enddate, status) " +
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

    /// <summary>
    /// Asynchronously updates the details of an existing project in the database.
    /// </summary>
    /// <param name="project">The project entity containing updated details to be saved.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task UpdateProjectAsync(Project project)
    {
        var sql = "UPDATE task_manager.project SET name = @Name, description = @Description, " +
                  "userid = @UserId, startdate = @StartDate, enddate = @EndDate, status = @Status WHERE id = @Id";

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

    /// <summary>
    /// Marks a project as deleted in the database by setting its status to 'Deleted'.
    /// </summary>
    /// <param name="id">The unique identifier of the project to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DeleteProjectAsync(int id)
    {
        var sql = "UPDATE task_manager.project set status = 'Deleted' WHERE id = @Id;";

        var parameters = new DynamicParameters();
        parameters.Add("Id", id, DbType.Int32);

        await _dataAccess.ExecuteAsync(sql, parameters);
    }
}