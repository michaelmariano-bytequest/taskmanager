using TaskManagerAPI.Core.Entities;

namespace TaskManagerAPI.Infrastructure.Interfaces;

public interface IProjectRepository
{
    Task<Project> GetProjectByIdAsync(int id);
    Task<IEnumerable<Project>> GetProjectsByUserIdAsync(int userId);
    Task<int> CreateProjectAsync(Project project);
    Task UpdateProjectAsync(Project project);
    Task DeleteProjectAsync(int id);
}