using System.Data;
using Dapper;
using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Infrastructure.DataAccess;
using TaskManagerAPI.Infrastructure.Interfaces;

namespace TaskManagerAPI.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public ReportRepository(ISqlDataAccess _sqlDataAccess)
        {
            _sqlDataAccess = _sqlDataAccess;
        }

        public async Task<List<UserPerformanceDTO>> GetCompletedTasksReportAsync(
            int? userId = null, 
            DateTime? startDate = null, 
            DateTime? endDate = null)
        {
            var sql = @"
                SELECT p.userid AS UserId, COUNT(*) / 30.0 AS AvgCompletedTasksPerDay
                FROM task_manager.todo_task tt 
                JOIN task_manager.project p ON tt.project_id = p.id
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
                sql += " AND tt.due_date >= @StartDate";
                parameters.Add("StartDate", startDate.Value);
            }
            else
            {
                sql += " AND tt.due_date >= NOW() - INTERVAL '30 days'";
            }

            // Filtro de data final
            if (endDate.HasValue)
            {
                sql += " AND tt.due_date <= @EndDate";
                parameters.Add("EndDate", endDate.Value);
            }

            sql += " GROUP BY p.userid;";

            return await _sqlDataAccess.QueryAsync<UserPerformanceDTO>(sql, parameters);
        }
    }
}