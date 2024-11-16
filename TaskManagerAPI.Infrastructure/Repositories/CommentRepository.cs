using System.Data;
using Dapper;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Infrastructure.DataAccess;
using TaskManagerAPI.Infrastructure.Interfaces;

namespace TaskManagerAPI.Infrastructure.Repositories
{
    /// <summary>
    /// Repository class for managing comments within the TaskManagerAPI.
    /// </summary>
    public class CommentRepository : ICommentRepository
    {
        /// <summary>
        /// An instance of the ISqlDataAccess interface used for executing SQL queries
        /// and commands asynchronously within the repository.
        /// </summary>
        private readonly ISqlDataAccess _sqlDataAccess;

        /// <summary>
        /// Repository for managing comments in the TaskManagerAPI.
        /// </summary>
        public CommentRepository(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        /// <summary>
        /// Asynchronously adds a new comment to the database.
        /// </summary>
        /// <param name="comment">The comment entity to be added to the database.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
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