using TaskManagerAPI.Core.Entities;

namespace TaskManagerAPI.Infrastructure.Interfaces;

/// <summary>
/// Represents a repository interface for accessing project data.
/// </summary>
public interface IProjectRepository
{
    Task<Project> GetProjectByIdAsync(int id);
    Task<List<Project>> GetProjectsByUserIdAsync(int userId);
    Task<int> CreateProjectAsync(Project project);
    Task UpdateProjectAsync(Project project);
    Task DeleteProjectAsync(int id);
}