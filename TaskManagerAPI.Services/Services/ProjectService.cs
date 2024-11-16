using TaskManagerAPI.Core.Common;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Core.Enums;
using TaskManagerAPI.Infrastructure.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services.Services;

/// <summary>
/// Service class responsible for managing project-related operations.
/// </summary>
public class ProjectService : IProjectService
{
    /// <summary>
    /// Repository for handling project data operations.
    /// </summary>
    private readonly IProjectRepository _projectRepository;

    /// <summary>
    /// Repository interface for handling operations related to TodoTask entities.
    /// </summary>
    private readonly ITodoTaskRepository _todoTaskRepository;

    /// <summary>
    /// The ProjectService class provides methods to manage projects within the TaskManagerAPI.
    /// </summary>
    public ProjectService(IProjectRepository projectRepository, ITodoTaskRepository todoTaskRepository)
    {
        _projectRepository = projectRepository;
        _todoTaskRepository = todoTaskRepository;
    }

    /// <summary>
    /// Retrieves a project by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the project.</param>
    /// <returns>A task that represents the asynchronous operation, containing the result of type Project.</returns>
    public async Task<Result<Project>> GetProjectByIdAsync(int id)
    {
        var project = await _projectRepository.GetProjectByIdAsync(id);

        if (project == null)
        {
            return Result<Project>.Failure("Project not found.");
        }

        return Result<Project>.Success(project);
    }

    /// <summary>
    /// Asynchronously retrieves a list of projects associated with a specific user ID.
    /// </summary>
    /// <param name="userId">The ID of the user whose projects are to be retrieved.</param>
    /// <returns>A result object containing a list of projects associated with the user ID if successful, otherwise an error message.</returns>
    public async Task<Result<List<Project>>> GetProjectsByUserIdAsync(int userId)
    {
        var projects = await _projectRepository.GetProjectsByUserIdAsync(userId);
        
        if (!projects.Any())
            return Result<List<Project>>.Failure("Project not found by userId.");
        else
            return Result<List<Project>>.Success(projects);
    }

    /// <summary>
    /// Asynchronously creates a new project.
    /// </summary>
    /// <param name="project">The project entity to be created.</param>
    /// <returns>A <see cref="Result"/> object containing the success status and the id of the newly created project.</returns>
    public async Task<Result> CreateProjectAsync(Project project)
    {
        int id = await _projectRepository.CreateProjectAsync(project);
        project.Id = id;
        
        return Result<int>.Success(id);
    }

    /// <summary>
    /// Updates an existing project asynchronously.
    /// </summary>
    /// <param name="project">The project to update.</param>
    /// <returns>A Result object indicating the success or failure of the operation.</returns>
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

    /// <summary>
    /// Deletes a project asynchronously. If the project has pending tasks, it will not be deleted.
    /// </summary>
    /// <param name="id">The identifier of the project to be deleted.</param>
    /// <returns>A <see cref="Result"/> indicating the success or failure of the operation.</returns>
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