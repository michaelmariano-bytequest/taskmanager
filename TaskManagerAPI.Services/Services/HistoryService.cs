using System.Reflection;
using System.Text;
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

    public async Task<List<History>> GetHistoryByTaskIdAsync(int taskId)
    {
        return await _historyRepository.GetHistoryByTaskIdAsync(taskId);
    }
}