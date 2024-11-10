using TaskManagerAPI.DataAccess;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Interfaces;

namespace TaskManagerAPI.Repositories
{
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
            return await _dataAccess.QuerySingleAsync<User>(sql, new { Id = id });
        }

        public async Task CreateUserAsync(User user)
        {
            var sql = "INSERT INTO task_manager.user (name, email, password_hash, created_at) " +
                      "VALUES (@Name, @Email, @PasswordHash, @CreatedAt)";
            await _dataAccess.ExecuteAsync(sql, user);
        }
    }
}