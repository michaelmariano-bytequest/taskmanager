using System.Data;
using Dapper;

namespace TaskManagerAPI.Infrastructure.DataAccess;

/// <summary>
/// Provides methods for interacting with a SQL database using Dapper.
/// </summary>
public class SqlDataAccess : ISqlDataAccess
{
    private readonly IDbConnection _dbConnection;

    /// <summary>
    /// Provides methods for interacting with a SQL database using Dapper.
    /// </summary>
    public SqlDataAccess(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    /// <summary>
    /// Executes an asynchronous query and returns the result as a list of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the result list.</typeparam>
    /// <param name="sql">The SQL query to execute.</param>
    /// <param name="parameters">The parameters for the SQL query.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of elements of type T.</returns>
    public async Task<List<T>> QueryAsync<T>(string sql, DynamicParameters parameters)
    {
        return (await _dbConnection.QueryAsync<T>(sql, parameters)).ToList();
    }

    /// <summary>
    /// Executes the provided SQL query and returns a single result asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the result object.</typeparam>
    /// <param name="sql">The SQL query string to be executed.</param>
    /// <param name="parameters">The parameters for the SQL query.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the single result object of type T. If no result is found, it returns the default value of type T.</returns>
    public async Task<T> QuerySingleAsync<T>(string sql, DynamicParameters parameters)
    {
        return await _dbConnection.QuerySingleOrDefaultAsync<T>(sql, parameters);
    }

    /// <summary>
    /// Executes a SQL command asynchronously and returns the number of affected rows.
    /// </summary>
    /// <param name="sql">The SQL command to be executed.</param>
    /// <param name="parameters">The parameters for the SQL command.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of rows affected.</returns>
    public async Task<int> ExecuteAsync(string sql, DynamicParameters parameters)
    {
        return await _dbConnection.ExecuteAsync(sql, parameters);
    }

    /// <summary>
    /// Executes a SQL query and returns the result as a scalar value asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the scalar result.</typeparam>
    /// <param name="sql">The SQL query to be executed.</param>
    /// <param name="parameters">The parameters to be passed to the SQL query.</param>
    /// <returns>A task representing the asynchronous operation, containing the scalar result of the query.</returns>
    public async Task<T> ExecuteScalarAsync<T>(string sql, DynamicParameters parameters)
    {
        return await _dbConnection.ExecuteScalarAsync<T>(sql, parameters);
    }
}