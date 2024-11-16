using System.Data;
using Dapper;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Infrastructure.DataAccess;
using TaskManagerAPI.Infrastructure.Interfaces;

namespace TaskManagerAPI.Infrastructure.Repositories;

/// <summary>
/// The UserRepository class provides methods to interact with the user data in the database.
/// </summary>
public class UserRepository : IUserRepository
{
    /// <summary>
    /// Instance of ISqlDataAccess used for executing SQL queries against the database.
    /// </summary>
    private readonly ISqlDataAccess _dataAccess;

    /// <summary>
    /// Repository class for performing CRUD operations on Users.
    /// </summary>
    public UserRepository(ISqlDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user with the specified identifier, if found.</returns>
    public async Task<User> GetUserByIdAsync(int id)
    {
        var sql = "SELECT * FROM task_manager.user WHERE id = @Id";

        var parameters = new DynamicParameters();
        parameters.Add("Id", id, DbType.Int32);

        return await _dataAccess.QuerySingleAsync<User>(sql, parameters);
    }

    /// <summary>
    /// Asynchronously creates a new user in the task manager system.
    /// </summary>
    /// <param name="user">The user entity containing user details such as name, email, password hash, and creation date.</param>
    /// <returns>The ID of the newly created user.</returns>
    public async Task<int> CreateUserAsync(User user)
    {
        var sql = "INSERT INTO task_manager.user (name, email, passwordhash, createdat) " +
                  "VALUES (@Name, @Email, @PasswordHash, @CreatedAt) RETURNING id;";

        var parameters = new DynamicParameters();
        parameters.Add("Name", user.Name, DbType.String);
        parameters.Add("Email", user.Email, DbType.String);
        parameters.Add("PasswordHash", user.PasswordHash, DbType.String);
        parameters.Add("CreatedAt", user.CreatedAt, DbType.DateTime);

        return await _dataAccess.QuerySingleAsync<int>(sql, parameters);
    }
}