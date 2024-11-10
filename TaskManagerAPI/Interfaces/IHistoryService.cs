using TaskManagerAPI.Common;

namespace TaskManagerAPI.Interfaces;

public interface IHistoryService
{
    Task<Result> AddHistoryEntryAsync(int taskId, string description);
}