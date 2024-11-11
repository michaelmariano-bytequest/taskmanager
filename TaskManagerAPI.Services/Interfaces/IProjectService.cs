using TaskManagerAPI.Core.Common;
using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Core.Entities;

namespace TaskManagerAPI.Services.Interfaces;

public interface IProjectService
{
    Task<Result<Project>> GetProjectByIdAsync(int id);
    Task<Result<List<Project>>> GetProjectsByUserIdAsync(int userId);
    Task<Result> CreateProjectAsync(Project project);
    Task<Result> UpdateProjectAsync(Project project);
    Task<Result> DeleteProjectAsync(int id);
}