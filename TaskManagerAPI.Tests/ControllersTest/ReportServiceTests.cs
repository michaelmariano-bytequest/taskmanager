using Xunit;
using Moq;
using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Infrastructure.Interfaces;
using TaskManagerAPI.Services.Services;

namespace TaskManagerAPI.Tests
{
    /// <summary>
    /// Test class for <see cref="ReportService"/>.
    /// Contains unit tests for methods to generate performance reports.
    /// </summary>
    public class ReportServiceTests
    {
        /// <summary>
        /// A mocked instance of IReportRepository used for testing purposes in the ReportServiceTests.
        /// This mock is utilized to set up expectations and return predefined data for methods in the IReportRepository,
        /// allowing the testing of the ReportService class without relying on the actual implementation of IReportRepository.
        /// </summary>
        private readonly Mock<IReportRepository> _mockReportRepository;

        /// <summary>
        /// Instance of ReportService used for generating various types of reports
        /// within the Task Manager API.
        /// </summary>
        private readonly ReportService _reportService;

        /// <summary>
        /// Unit tests for the ReportService class.
        /// </summary>
        public ReportServiceTests()
        {
            _mockReportRepository = new Mock<IReportRepository>();
            _reportService = new ReportService(_mockReportRepository.Object);
        }

        /// <summary>
        /// Unit test for GeneratePerformanceReportAsync method in ReportService.
        /// Verifies that the method returns a valid PerformanceReportDTO when provided with correct data.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the PerformanceReportDTO object.
        /// </returns>
        [Fact]
        public async Task GeneratePerformanceReportAsync_WithValidData_ReturnsPerformanceReportDTO()
        {
            // Arrange
            var userId = 1;
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2023, 12, 31);
            var reportData = new List<UserPerformanceDTO>
            {
                new UserPerformanceDTO { UserId = 1, AvgCompletedTasksPerDay = 5.5 }
            };

            _mockReportRepository
                .Setup(repo => repo.GetCompletedTasksReportAsync(userId, startDate, endDate))
                .ReturnsAsync(reportData);

            // Act
            var result = await _reportService.GeneratePerformanceReportAsync(userId, startDate, endDate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(reportData, result.Data);
        }

        /// <summary>
        /// Tests the GeneratePerformanceReportAsync method to ensure it returns a PerformanceReportDTO
        /// when called with a null userId parameter.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains
        /// a PerformanceReportDTO with the expected report data.
        /// </returns>
        [Fact]
        public async Task GeneratePerformanceReportAsync_WithNullUserId_ReturnsPerformanceReportDTO()
        {
            // Arrange
            int? userId = null;
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2023, 12, 31);
            var reportData = new List<UserPerformanceDTO>
            {
                new UserPerformanceDTO { UserId = 2, AvgCompletedTasksPerDay = 3.2 }
            };

            _mockReportRepository
                .Setup(repo => repo.GetCompletedTasksReportAsync(userId, startDate, endDate))
                .ReturnsAsync(reportData);

            // Act
            var result = await _reportService.GeneratePerformanceReportAsync(userId, startDate, endDate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(reportData, result.Data);
        }

        // Adiciona testes adicionais conforme necessário para cobrir diferentes casos de uso
    }
}