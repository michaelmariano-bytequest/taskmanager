using Dapper;

namespace TaskManagerAPI.Infrastructure.DataAccess;

public interface ISqlDataAccess
{
    Task<List<T>> QueryAsync<T>(string sql, DynamicParameters parameters);
    Task<T> QuerySingleAsync<T>(string sql, DynamicParameters parameters);
    Task<int> ExecuteAsync(string sql, DynamicParameters parameters);
    Task<T> ExecuteScalarAsync<T>(string sql, DynamicParameters parameters);
}