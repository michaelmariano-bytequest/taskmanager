using System.Reflection;
using System.Text;
using TaskManagerAPI.Core.Common;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Infrastructure.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services.Services;

/// <summary>
/// Service responsible for managing history entries related to tasks.
/// </summary>
public class HistoryService : IHistoryService
{
    /// <summary>
    /// Represents the repository interface for accessing and managing task history records.
    /// </summary>
    private readonly IHistoryRepository _historyRepository;

    /// <summary>
    /// Service for managing and retrieving history entries associated with tasks.
    /// </summary>
    public HistoryService(IHistoryRepository historyRepository)
    {
        _historyRepository = historyRepository;
    }

    /// <summary>
    /// Adds a new history entry for a specified task with a given description and associated data.
    /// </summary>
    /// <param name="taskId">The identifier of the task to which the history entry is related.</param>
    /// <param name="description">A text description of the history entry.</param>
    /// <param name="obj">An object containing properties that will be appended to the description. If it contains a 'UserId' property, it will be included in the history entry.</param>
    /// <returns>A result indicating the success or failure of the operation.</returns>
    public async Task<Result> AddHistoryEntryAsync(int taskId, string description, object obj)
    {
        // Constrói a descrição com as propriedades de `obj`
        var extendedDescription = new StringBuilder(description);
        int userId = 0;
        
        if (obj != null)
        {
            extendedDescription.Append(" | ");

            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                if(propertyName == "UserId")
                    userId = (int) property.GetValue(obj);
                
                var propertyValue = property.GetValue(obj) ?? "null"; // Se o valor for nulo, usa "null" como representação
                extendedDescription.Append($"{propertyName}: {propertyValue}, ");
            }

            // Remove a última vírgula e espaço
            if (properties.Length > 0)
                extendedDescription.Length -= 2;
        }

        var historyEntry = new History
        {
            TaskId = taskId,
            Description = extendedDescription.ToString(),
            ModifiedAt = DateTime.UtcNow,
            UserId = userId
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

    /// <summary>
    /// Asynchronously retrieves the history records associated with a given task ID.
    /// </summary>
    /// <param name="taskId">The unique identifier of the task whose history is being retrieved.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the list of history records related to the specified task ID.</returns>
    public async Task<List<History>> GetHistoryByTaskIdAsync(int taskId)
    {
        return await _historyRepository.GetHistoryByTaskIdAsync(taskId);
    }
}