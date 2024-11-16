using Moq;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Infrastructure.Interfaces;
using TaskManagerAPI.Services.Services;
using Xunit;

namespace TaskManagerAPI.Tests.Services
{
    /// <summary>
    /// Contains unit tests for <see cref="HistoryService"/> class.
    /// </summary>
    public class HistoryServiceTests
    {
        /// <summary>
        /// Mock implementation of <see cref="IHistoryRepository"/> used for testing purposes in the <see cref="HistoryServiceTests"/> class.
        /// </summary>
        private readonly Mock<IHistoryRepository> _historyRepositoryMock;

        /// <summary>
        /// The HistoryService instance used for managing and accessing history entries
        /// related to tasks in the unit tests.
        /// </summary>
        private readonly HistoryService _historyService;

        /// <summary>
        /// Unit tests for the HistoryService class, verifying its behavior
        /// when adding and retrieving history entries.
        /// </summary>
        public HistoryServiceTests()
        {
            _historyRepositoryMock = new Mock<IHistoryRepository>();
            _historyService = new HistoryService(_historyRepositoryMock.Object);
        }

        /// <summary>
        /// Adds a history entry asynchronously and returns a success result if the entry is added successfully.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with a result indicating the success status.</returns>
        [Fact]
        public async Task AddHistoryEntryAsync_ShouldReturnSuccessResult_WhenEntryIsAdded()
        {
            // Arrange
            var taskId = 1;
            var description = "Task completed";
            var associatedData = new { UserId = 123, Detail = "details" };

            _historyRepositoryMock
                .Setup(x => x.AddHistoryAsync(It.IsAny<History>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _historyService.AddHistoryEntryAsync(taskId, description, associatedData);

            // Assert
            Assert.True(result.IsSuccess);
            _historyRepositoryMock.Verify(x => x.AddHistoryAsync(It.IsAny<History>()), Times.Once);
        }

        /// <summary>
        /// Ensures that the AddHistoryEntryAsync method in HistoryService returns a failure Result
        /// when an exception is thrown during the process of adding a history entry.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains
        /// a failure Result indicating that the history entry could not be added and contains
        /// an appropriate error message.
        /// </returns>
        [Fact]
        public async Task AddHistoryEntryAsync_ShouldReturnFailureResult_WhenExceptionIsThrown()
        {
            // Arrange
            var taskId = 1;
            var description = "Task completed";
            var associatedData = new { UserId = 123, Detail = "details" };

            _historyRepositoryMock
                .Setup(x => x.AddHistoryAsync(It.IsAny<History>()))
                .Throws(new Exception("Database error"));

            // Act
            var result = await _historyService.AddHistoryEntryAsync(taskId, description, associatedData);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to add history entry: Database error", result.ErrorMessage);
            _historyRepositoryMock.Verify(x => x.AddHistoryAsync(It.IsAny<History>()), Times.Once);
        }

        /// <summary>
        /// Verifies that the GetHistoryByTaskIdAsync method returns the correct list of history entries for a given task ID.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task GetHistoryByTaskIdAsync_ShouldReturnHistoryList()
        {
            // Arrange
            var taskId = 1;
            var historyList = new List<History>
            {
                new History { TaskId = taskId, Description = "Task created", UserId = 123 },
                new History { TaskId = taskId, Description = "Task updated", UserId = 123 }
            };

            _historyRepositoryMock
                .Setup(x => x.GetHistoryByTaskIdAsync(taskId))
                .ReturnsAsync(historyList);

            // Act
            var result = await _historyService.GetHistoryByTaskIdAsync(taskId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Task created", result[0].Description);
            _historyRepositoryMock.Verify(x => x.GetHistoryByTaskIdAsync(taskId), Times.Once);
        }
    }
}