using TaskManagerAPI.Core.Entities;

namespace TaskManagerAPI.Infrastructure.Interfaces;

public interface IHistoryRepository
{
    Task<List<History>> GetHistoryByTaskIdAsync(int taskId);
    Task AddHistoryAsync(History history);
}