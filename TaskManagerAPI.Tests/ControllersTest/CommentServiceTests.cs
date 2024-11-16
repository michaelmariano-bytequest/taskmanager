using Moq;
using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Infrastructure.Interfaces;
using TaskManagerAPI.Services.Interfaces;
using TaskManagerAPI.Services.Services;
using Xunit;

namespace TaskManagerAPI.Tests.Services
{
    /// <summary>
    /// Contains unit tests for the CommentService implementation.
    /// </summary>
    public class CommentServiceTests
    {
        /// <summary>
        /// Tests the AddCommentAndLogHistoryAsync method to ensure that a comment is added
        /// and the corresponding history entry is logged.
        /// </summary>
        /// <returns>A task that represents the asynchronous test operation.</returns>
        [Fact]
        public async Task AddCommentAndLogHistoryAsync_ShouldAddCommentAndHistory()
        {
            // Arrange
            var commentRepositoryMock = new Mock<ICommentRepository>();
            var historyServiceMock = new Mock<IHistoryService>();

            var commentService = new CommentService(commentRepositoryMock.Object, historyServiceMock.Object);

            var commentDto = new AddCommentDTO
            {
                TaskId = 1,
                UserId = 1,
                CommentText = "This is a test comment"
            };

            // Act
            await commentService.AddCommentAndLogHistoryAsync(commentDto);

            // Assert
            // Verifica se AddCommentAsync foi chamada com os parâmetros corretos
            commentRepositoryMock.Verify(repo => repo.AddCommentAsync(It.Is<Comment>(c =>
                c.TaskId == commentDto.TaskId &&
                c.UserId == commentDto.UserId &&
                c.CommentText == commentDto.CommentText &&
                c.CreatedAt <= DateTime.UtcNow)), Times.Once);

            // Verifica se AddHistoryEntryAsync foi chamada com os parâmetros corretos
            var expectedDescription = $"Comment added by user {commentDto.UserId}: {commentDto.CommentText}";
            historyServiceMock.Verify(
                history => history.AddHistoryEntryAsync(commentDto.TaskId, expectedDescription, commentDto),
                Times.Once);
        }
    }
}