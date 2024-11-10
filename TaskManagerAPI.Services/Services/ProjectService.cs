using TaskManagerAPI.Core.Common;
using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Infrastructure.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly ITodoTaskRepository _todoTaskRepository;

    public ProjectService(IProjectRepository projectRepository, ITodoTaskRepository todoTaskRepository)
    {
        _projectRepository = projectRepository;
        _todoTaskRepository = todoTaskRepository;
    }

    public async Task<Result<Project>> GetProjectByIdAsync(int id)
    {
        var project = await _projectRepository.GetProjectByIdAsync(id);

        if (project == null)
        {
            return Result<Project>.Failure("Project not found.");
        }

        return Result<Project>.Success(project);
    }

    public async Task<Result<IEnumerable<Project>>> GetProjectsByUserIdAsync(int userId)
    {
        var projects = await _projectRepository.GetProjectsByUserIdAsync(userId);
        
        if (!projects.Any())
            return Result<IEnumerable<Project>>.Failure("Project not found by userId.");
        else
            return Result<IEnumerable<Project>>.Success(projects);
    }

    public async Task<Result> CreateProjectAsync(Project project)
    {
        int id = await _projectRepository.CreateProjectAsync(project);
        project.Id = id;
        
        return Result<int>.Success(id);
    }

    public async Task<Result> UpdateProjectAsync(Project project)
    {
        var existingProject = await _projectRepository.GetProjectByIdAsync(project.Id);

        if (existingProject == null)
        {
            return Result.Failure("Project not found.");
        }

        await _projectRepository.UpdateProjectAsync(project);
        return Result.Success();
    }

    public async Task<Result> DeleteProjectAsync(int id)
    {
        var project = await _projectRepository.GetProjectByIdAsync(id);

        if (project == null)
        {
            return Result.Failure("Project not found.");
        }

        var tasks = await _todoTaskRepository.GetTasksByProjectIdAsync(id);
        if (tasks.Any())
        {
            return Result.Failure("Cannot delete project with associated tasks.");
        }

        await _projectRepository.DeleteProjectAsync(id);
        return Result.Success();
    }
}