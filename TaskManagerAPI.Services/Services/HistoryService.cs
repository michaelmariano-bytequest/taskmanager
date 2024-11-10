using TaskManagerAPI.Core.Common;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Infrastructure.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services.Services;

public class HistoryService : IHistoryService
{
    private readonly IHistoryRepository _historyRepository;

    public HistoryService(IHistoryRepository historyRepository)
    {
        _historyRepository = historyRepository;
    }

    public async Task<Result> AddHistoryEntryAsync(int taskId, string description)
    {
        var historyEntry = new History
        {
            TaskId = taskId,
            Description = description,
            ModifiedAt = DateTime.UtcNow
        };

        try
        {
            await _historyRepository.AddHistoryAsync(historyEntry);
            return Result.Success();
        }
        catch (Exception ex)
        {
            // Log error here if needed
            return Result.Failure($"Failed to add history entry: {ex.Message}");
        }
    }
}