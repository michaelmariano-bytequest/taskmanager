using TaskManagerAPI.Core.Common;
using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Core.Enums;
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

    public async Task<Result<List<Project>>> GetProjectsByUserIdAsync(int userId)
    {
        var projects = await _projectRepository.GetProjectsByUserIdAsync(userId);
        
        if (!projects.Any())
            return Result<List<Project>>.Failure("Project not found by userId.");
        else
            return Result<List<Project>>.Success(projects);
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

        var tasks = await _todoTaskRepository.GetTasksByProjectIdAndStatusAsync(id, 
            TodoTaskStatusEnum.Pending);
       
        if (tasks.Any())
        {
            return Result.Failure("Cannot delete the project because there are pending tasks. Please complete or " +
                                  "remove all tasks associated with the project before attempting to delete it.");
        }

        await _projectRepository.DeleteProjectAsync(id);
        
        return Result.Success();
    }
}