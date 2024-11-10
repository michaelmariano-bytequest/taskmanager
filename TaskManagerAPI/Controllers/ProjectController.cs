using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Interfaces;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        /// <summary>
        /// Retrieve a project by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the project.</param>
        /// <returns>The project details.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProjectById(int id)
        {
            var project = await _projectRepository.GetProjectByIdAsync(id);
            if (project == null)
                return NotFound();
            return Ok(project);
        }

        /// <summary>
        /// Retrieve all projects for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjectsByUserId(int userId)
        {
            var projects = await _projectRepository.GetProjectsByUserIdAsync(userId);
            return Ok(projects);
        }

        /// <summary>
        /// Create a new project.
        /// </summary>
        /// <param name="project">The project details to create.</param>
        [HttpPost]
        public async Task<IActionResult> CreateProject(Project project)
        {
            await _projectRepository.CreateProjectAsync(project);
            return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, project);
        }

        /// <summary>
        /// Update an existing project.
        /// </summary>
        /// <param name="id">The ID of the project to update.</param>
        /// <param name="project">The updated project details.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, Project project)
        {
            if (id != project.Id)
                return BadRequest();
            await _projectRepository.UpdateProjectAsync(project);
            return NoContent();
        }

        /// <summary>
        /// Delete a project by ID.
        /// </summary>
        /// <param name="id">The ID of the project to delete.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            await _projectRepository.DeleteProjectAsync(id);
            return NoContent();
        }
    }
}