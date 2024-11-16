using Dapper;

namespace TaskManagerAPI.Infrastructure.DataAccess;

/// <summary>
/// Interface for SQL data access.
/// Provides methods for executing SQL queries asynchronously.
/// </summary>
public interface ISqlDataAccess
{
    Task<List<T>> QueryAsync<T>(string sql, DynamicParameters parameters);
    Task<T> QuerySingleAsync<T>(string sql, DynamicParameters parameters);
    Task<int> ExecuteAsync(string sql, DynamicParameters parameters);
    Task<T> ExecuteScalarAsync<T>(string sql, DynamicParameters parameters);
}