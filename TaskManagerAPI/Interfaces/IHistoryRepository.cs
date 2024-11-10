using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Interfaces
{
    public interface IHistoryRepository
    {
        Task<IEnumerable<History>> GetHistoryByTaskIdAsync(int taskId);
        Task AddHistoryAsync(History history);
    }
}