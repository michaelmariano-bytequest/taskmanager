using TaskManagerAPI.Core.Common;
using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Core.Entities;

namespace TaskManagerAPI.Services.Interfaces;

public interface IHistoryService
{
    Task<Result> AddHistoryEntryAsync(int taskId, string description, object obj);
    Task<IEnumerable<History>> GetHistoryByTaskIdAsync(int taskId);
}