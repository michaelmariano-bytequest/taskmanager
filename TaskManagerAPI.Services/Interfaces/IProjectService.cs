using TaskManagerAPI.Core.Common;
using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Core.Entities;

namespace TaskManagerAPI.Services.Interfaces;

/// <summary>
/// Provides methods for managing project data.
/// </summary>
public interface IProjectService
{
    /// <summary>
    /// Asynchronously retrieves a project by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the project.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// a <see cref="Result{T}"/> object that contains the project if found, otherwise
    /// an error message.
    /// </returns>
    Task<Result<Project>> GetProjectByIdAsync(int id);

    /// <summary>
    /// Asynchronously retrieves the list of projects associated with a specific user ID.
    /// </summary>
    /// <param name="userId">The ID of the user whose projects are to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a Result object which includes a list of projects on success or an error message on failure.</returns>
    Task<Result<List<Project>>> GetProjectsByUserIdAsync(int userId);

    /// <summary>
    /// Asynchronously creates a new project.
    /// </summary>
    /// <param name="project">The project object containing details to be created.</param>
    /// <returns>A task that represents the result of the asynchronous operation.
    /// The task result contains a Result object indicating success or failure.</returns>
    Task<Result> CreateProjectAsync(Project project);

    /// <summary>
    /// Updates an existing project asynchronously with the provided project details.
    /// </summary>
    /// <param name="project">The project object containing updated information.</param>
    /// <returns>A <see cref="Task{Result}"/> representing the asynchronous operation, containing a
    /// <see cref="Result"/> indicating the success or failure of the operation.</returns>
    Task<Result> UpdateProjectAsync(Project project);

    /// <summary>
    /// Asynchronously deletes a project by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the project to be deleted.</param>
    /// <returns>A Result object indicating the success or failure of the operation.</returns>
    Task<Result> DeleteProjectAsync(int id);
}