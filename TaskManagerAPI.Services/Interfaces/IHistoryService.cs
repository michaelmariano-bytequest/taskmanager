using TaskManagerAPI.Core.Common;

namespace TaskManagerAPI.Services.Interfaces;

public interface IHistoryService
{
    Task<Result> AddHistoryEntryAsync(int taskId, string description);
}