using System.Data;
using Dapper;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Infrastructure.DataAccess;
using TaskManagerAPI.Infrastructure.Interfaces;

namespace TaskManagerAPI.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public CommentRepository(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task AddCommentAsync(Comment comment)
        {
            var sql = @"
                INSERT INTO task_manager.comments (taskid, userid, commenttext, createdat)
                VALUES (@TaskId, @UserId, @CommentText, @CreatedAt);";

            var parameters = new DynamicParameters();
            parameters.Add("TaskId", comment.TaskId, DbType.Int32);
            parameters.Add("UserId", comment.UserId, DbType.Int32);
            parameters.Add("CommentText", comment.CommentText, DbType.String);
            parameters.Add("CreatedAt", comment.CreatedAt, DbType.DateTime);

            await _sqlDataAccess.ExecuteAsync(sql, parameters);
        }
    }
}