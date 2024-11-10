using System.Data;
using Dapper;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Infrastructure.DataAccess;
using TaskManagerAPI.Infrastructure.Interfaces;

namespace TaskManagerAPI.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ISqlDataAccess _dataAccess;

    public UserRepository(ISqlDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        var sql = "SELECT * FROM task_manager.user WHERE id = @Id";

        var parameters = new DynamicParameters();
        parameters.Add("Id", id, DbType.Int32);

        return await _dataAccess.QuerySingleAsync<User>(sql, parameters);
    }

    public async Task CreateUserAsync(User user)
    {
        var sql = "INSERT INTO task_manager.user (name, email, password_hash, created_at) " +
                  "VALUES (@Name, @Email, @PasswordHash, @CreatedAt)";

        var parameters = new DynamicParameters();
        parameters.Add("Name", user.Name, DbType.String);
        parameters.Add("Email", user.Email, DbType.String);
        parameters.Add("PasswordHash", user.PasswordHash, DbType.String);
        parameters.Add("CreatedAt", user.CreatedAt, DbType.DateTime);

        await _dataAccess.ExecuteAsync(sql, parameters);
    }
}