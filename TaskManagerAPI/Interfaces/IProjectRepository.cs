using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project> GetProjectByIdAsync(int id);
        Task<IEnumerable<Project>> GetProjectsByUserIdAsync(int userId);
        Task CreateProjectAsync(Project project);
        Task UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(int id);
    }
}