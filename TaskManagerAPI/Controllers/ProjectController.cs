using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var result = await _projectService.GetProjectByIdAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound(new { Message = result.ErrorMessage });
            }

            return Ok(result.Value);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetProjectsByUserId(int userId)
        {
            var result = await _projectService.GetProjectsByUserIdAsync(userId);
            
            if (!result.IsSuccess)
                return NotFound(new { Message = result.ErrorMessage });
            else
                return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] Project project)
        {
            var result = await _projectService.CreateProjectAsync(project);

            if (!result.IsSuccess)
            {
                return BadRequest(new { Message = result.ErrorMessage });
            }

            return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, project);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] Project project)
        {
            project.Id = id; // Ensures the ID in the route is used
            var result = await _projectService.UpdateProjectAsync(project);

            if (!result.IsSuccess)
            {
                return NotFound(new { Message = result.ErrorMessage });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var result = await _projectService.DeleteProjectAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(new { Message = result.ErrorMessage });
            }

            return NoContent();
        }
    }
}