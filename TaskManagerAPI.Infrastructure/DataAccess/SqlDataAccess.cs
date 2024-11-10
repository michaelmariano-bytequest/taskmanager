using System.Data;
using Dapper;

namespace TaskManagerAPI.Infrastructure.DataAccess;

public class SqlDataAccess : ISqlDataAccess
{
    private readonly IDbConnection _dbConnection;

    public SqlDataAccess(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, DynamicParameters parameters)
    {
        return await _dbConnection.QueryAsync<T>(sql, parameters);
    }

    public async Task<T> QuerySingleAsync<T>(string sql, DynamicParameters parameters)
    {
        return await _dbConnection.QuerySingleOrDefaultAsync<T>(sql, parameters);
    }

    public async Task<int> ExecuteAsync(string sql, DynamicParameters parameters)
    {
        return await _dbConnection.ExecuteAsync(sql, parameters);
    }

    public async Task<T> ExecuteScalarAsync<T>(string sql, DynamicParameters parameters)
    {
        return await _dbConnection.ExecuteScalarAsync<T>(sql, parameters);
    }
}