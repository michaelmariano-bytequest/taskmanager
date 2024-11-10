using TaskManagerAPI.Core.Entities;

namespace TaskManagerAPI.Infrastructure.Interfaces;

public interface IHistoryRepository
{
    Task<IEnumerable<History>> GetHistoryByTaskIdAsync(int taskId);
    Task AddHistoryAsync(History history);
}