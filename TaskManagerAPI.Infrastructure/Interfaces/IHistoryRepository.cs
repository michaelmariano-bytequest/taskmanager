using TaskManagerAPI.Core.Entities;

namespace TaskManagerAPI.Infrastructure.Interfaces;

/// <summary>
/// Interface for accessing and managing task history records in the repository.
/// </summary>
public interface IHistoryRepository
{
    Task<List<History>> GetHistoryByTaskIdAsync(int taskId);
    Task AddHistoryAsync(History history);
}