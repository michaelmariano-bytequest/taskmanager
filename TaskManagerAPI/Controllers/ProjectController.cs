using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Core.Common;
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
        public async Task<ActionResult<Project>> GetProjectById(int id)
        {
            var result = await _projectService.GetProjectByIdAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.Value);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Project>>> GetProjectsByUserId(int userId)
        {
            var result = await _projectService.GetProjectsByUserIdAsync(userId);
            
            if (!result.IsSuccess)
                return NotFound(new { Message = result.ErrorMessage });
            else
                return Ok(result.Value);
        }

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