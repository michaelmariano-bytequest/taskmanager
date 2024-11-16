using TaskManagerAPI.Core.Common;
using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Core.Entities;

namespace TaskManagerAPI.Services.Interfaces;

/// <summary>
/// Provides functionality to manage and retrieve history entries associated with tasks.
/// </summary>
public interface IHistoryService
{
    /// <summary>
    /// Adds a history entry to the specified task.
    /// </summary>
    /// <param name="taskId">The ID of the task to which the history entry will be added.</param>
    /// <param name="description">The description of the history entry.</param>
    /// <param name="obj">Additional object containing details to be included in the history entry.</param>
    /// <returns>A <see cref="Result"/> indicating the success or failure of the operation.</returns>
    Task<Result> AddHistoryEntryAsync(int taskId, string description, object obj);

    /// <summary>
    /// Retrieves the history of changes for a specific task.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <returns>A list of history records for the specified task.</returns>
    Task<List<History>> GetHistoryByTaskIdAsync(int taskId);
}