using System.Data;
using Dapper;
using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Infrastructure.DataAccess;
using TaskManagerAPI.Infrastructure.Interfaces;

namespace TaskManagerAPI.Infrastructure.Repositories
{
    /// <summary>
    /// The ReportRepository class provides methods to access and generate various reports
    /// related to user performance and task completion statistics from the database.
    /// </summary>
    public class ReportRepository : IReportRepository
    {
        /// <summary>
        /// Private readonly instance of <see cref="ISqlDataAccess"/> used for executing SQL queries.
        /// </summary>
        private readonly ISqlDataAccess _sqlDataAccess;

        /// <summary>
        /// Repository for retrieving report data related to task management and user performance.
        /// </summary>
        public ReportRepository(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        /// <summary>
        /// Generates a report of completed tasks for users. The report includes the average number of tasks completed per day for each user within the specified date range.
        /// </summary>
        /// <param name="userId">Optional user ID to filter the report by a specific user.</param>
        /// <param name="startDate">Optional start date to filter the report. If not provided, defaults to 30 days prior to the current date.</param>
        /// <param name="endDate">Optional end date to filter the report. If not provided, there is no upper limit on the date range.</param>
        /// <returns>A list of user performance data transfer objects containing user IDs and their average completed tasks per day.</returns>
        public async Task<List<UserPerformanceDTO>> GetCompletedTasksReportAsync(
            int? userId = null, 
            DateTime? startDate = null, 
            DateTime? endDate = null)
        {
            var sql = @"
                SELECT p.userid AS UserId, COUNT(*) / 30.0 AS AvgCompletedTasksPerDay
                FROM task_manager.todo_task tt 
                JOIN task_manager.project p ON tt.projectid = p.id
                WHERE tt.status = 'Completed' 
                ";

            var parameters = new DynamicParameters();

            // Filtro de `userId`, se fornecido
            if (userId.HasValue)
            {
                sql += " AND p.userid = @UserId";
                parameters.Add("UserId", userId.Value);
            }

            // Filtro de data inicial
            if (startDate.HasValue)
            {
                sql += " AND tt.duedate >= @StartDate";
                parameters.Add("StartDate", startDate.Value);
            }
            else
            {
                sql += " AND tt.duedate >= NOW() - INTERVAL '30 days'";
            }

            // Filtro de data final
            if (endDate.HasValue)
            {
                sql += " AND tt.duedate <= @EndDate";
                parameters.Add("EndDate", endDate.Value);
            }

            sql += " GROUP BY p.userid;";

            return await _sqlDataAccess.QueryAsync<UserPerformanceDTO>(sql, parameters);
        }
    }
}