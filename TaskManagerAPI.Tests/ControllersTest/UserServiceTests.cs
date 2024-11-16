using System.Reflection;
using Xunit;
using Moq;
using AutoMapper;
using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Infrastructure.Interfaces;
using TaskManagerAPI.Services.Services;

/// <summary>
/// Tests for UserService class.
/// </summary>
public class UserServiceTests
{
    /// <summary>
    /// Mock implementation of the IUserRepository interface for testing purposes.
    /// </summary>
    private readonly Mock<IUserRepository> _userRepositoryMock;

    /// <summary>
    /// Mock object for the AutoMapper interface used to map entities to DTOs and vice versa.
    /// </summary>
    private readonly Mock<IMapper> _mapperMock;

    /// <summary>
    /// Represents the service used for user-related operations in the application.
    /// </summary>
    private readonly UserService _userService;

    /// <summary>
    /// Unit tests for the UserService class.
    /// </summary>
    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _userService = new UserService(_userRepositoryMock.Object, _mapperMock.Object);
    }

    /// <summary>
    /// Validates that the GetUserByIdAsync method returns a user when the user exists in the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous unit test operation.</returns>
    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        int userId = 1;
        var user = new User { Id = userId, Name = "John Doe", Email = "john.doe@example.com" };
        var userDto = new UserDTO { Id = userId, Name = "John Doe", Email = "john.doe@example.com" };
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);
        _mapperMock.Setup(mapper => mapper.Map<UserDTO>(user)).Returns(userDto);

        // Act
        var result = await _userService.GetUserByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal("John Doe", result.Name);
    }

    /// <summary>
    /// Tests that a user ID is returned when a user is successfully created.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation. The task result is an integer representing the user ID.</returns>
    [Fact]
    public async Task CreateUserAsync_ShouldReturnUserId_WhenUserIsCreated()
    {
        // Arrange
        var createUserDto = new CreateUserDTO { Name = "New User", Email = "new.user@example.com", Password = "Password123" };
        var user = new User { Id = 1, Name = "New User", Email = "new.user@example.com", PasswordHash = "hashedpassword" };
        _mapperMock.Setup(mapper => mapper.Map<User>(createUserDto)).Returns(user);
        _userRepositoryMock.Setup(repo => repo.CreateUserAsync(user)).ReturnsAsync(user.Id);

        // Act
        var result = await _userService.CreateUserAsync(createUserDto);

        // Assert
        Assert.Equal(user.Id, result);
    }

    /// <summary>
    /// Tests the VerifyPassword method to ensure it returns the expected result.
    /// </summary>
    /// <param name="password">The password to verify.</param>
    /// <param name="expectedResult">The expected result of the password verification.</param>
    [Theory]
    [InlineData("password123", true)]  // Senha correta
    [InlineData("wrongpassword", false)] // Senha incorreta
    public void VerifyPassword_ShouldReturnExpectedResult(string password, bool expectedResult)
    {
        // Gerar um hash para o "password123"
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");

        // Usar reflection para acessar o método privado VerifyPassword
        var methodInfo = typeof(UserService).GetMethod("VerifyPassword", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(methodInfo);

        // Invocar o método usando reflection
        var result = (bool)methodInfo.Invoke(_userService, new object[] { password, hashedPassword });

        // Verificar se o resultado é o esperado
        Assert.Equal(expectedResult, result);
    }
}