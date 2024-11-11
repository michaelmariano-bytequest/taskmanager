using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    /// <summary>
    /// Add a comment to a task and log it in the history.
    /// </summary>
    /// <param name="commentDto">The comment details.</param>
    [HttpPost]
    public async Task<IActionResult> AddComment([FromBody] AddCommentDTO commentDto)
    {
        await _commentService.AddCommentAndLogHistoryAsync(commentDto);
        
        return Ok("Comment added successfully and logged in history.");
    }
}