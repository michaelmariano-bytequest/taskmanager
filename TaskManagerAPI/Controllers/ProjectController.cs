using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Core.Common;
using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        /// <summary>
        /// Initializes a new instance of the ProjectController class.
        /// </summary>
        /// <param name="projectService">Service for managing projects.</param>
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// Retrieves a project by its ID.
        /// </summary>
        /// <param name="id">The project's ID.</param>
        /// <returns>The project details.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProjectById(int id)
        {
            var result = await _projectService.GetProjectByIdAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Retrieves all projects for a specific user by their user ID.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <returns>A list of the user's projects.</returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Project>>> GetProjectsByUserId(int userId)
        {
            var result = await _projectService.GetProjectsByUserIdAsync(userId);
            
            if (!result.IsSuccess)
                return NotFound(new { Message = result.ErrorMessage });
            else
                return Ok(result.Value);
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="project">An object containing the project's data.</param>
        /// <returns>The created project with its generated ID.</returns>
        [HttpPost]
        public async Task<ActionResult> CreateProject([FromBody] Project project)
        {
            var result = await _projectService.CreateProjectAsync(project);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, project);
        }

        /// <summary>
        /// Updates an existing project.
        /// </summary>
        /// <param name="id">The ID of the project to update.</param>
        /// <param name="project">The updated project data.</param>
        /// <returns>No Content if the update is successful.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProject(int id, [FromBody] Project project)
        {
            project.Id = id; 
            var result = await _projectService.UpdateProjectAsync(project);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a project by its ID.
        /// </summary>
        /// <param name="id">The ID of the project to delete.</param>
        /// <returns>No Content if the deletion is successful.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProject(int id)
        {
            var result = await _projectService.DeleteProjectAsync(id);

            if (!result.IsSuccess)
            {
                var errorResponse = new ErrorResponse
                {
                    Message = result.ErrorMessage
                };
                return BadRequest(errorResponse);
            }

            return NoContent();
        }
    }
}