using System.Data;
using Dapper;
using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Infrastructure.Interfaces;

namespace TaskManagerAPI.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly IDbConnection _dbConnection;

        public ReportRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<UserPerformanceDTO>> GetCompletedTasksReportAsync()
        {
            var sql = @"
                SELECT p.userid AS UserId, COUNT(*) / 30.0 AS AvgCompletedTasksPerDay
                FROM task_manager.todo_task tt join task_manager.project p 
                ON tt.project_id = p.id
                WHERE tt.status = 'Completed' 
                AND tt.due_date >= NOW() - INTERVAL '30 days'
                GROUP BY p.userid;
            ";

            return await _dbConnection.QueryAsync<UserPerformanceDTO>(sql);
        }
    }
}