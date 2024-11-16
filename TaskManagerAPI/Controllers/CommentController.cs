using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller for handling operations related to comments on tasks.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CommentController : ControllerBase
{
    /// <summary>
    /// Service for managing comment operations including adding comments and logging them in task history.
    /// </summary>
    private readonly ICommentService _commentService;

    /// <summary>
    /// Controller for handling operations related to comments.
    /// </summary>
    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    /// <summary>
    /// Add a comment to a task and log it in the history.
    /// </summary>
    /// <param name="commentDto">The comment details.</param>
    /// <returns>An ActionResult indicating the outcome of the operation.</returns>
    [HttpPost]
    public async Task<ActionResult> AddComment([FromBody] AddCommentDTO commentDto)
    {
        await _commentService.AddCommentAndLogHistoryAsync(commentDto);
        
        return Ok("Comment added successfully and logged in history.");
    }
}