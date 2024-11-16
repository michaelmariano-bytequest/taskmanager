using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagerAPI.Controllers;
using TaskManagerAPI.Core.Common;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Core.Enums;
using TaskManagerAPI.Infrastructure.Interfaces;
using TaskManagerAPI.Services.Interfaces;
using TaskManagerAPI.Services.Services;
using Xunit;


namespace TaskManagerAPI.Tests.ControllersTest;

/// <summary>
/// Unit tests for the ProjectController class.
/// </summary>
public class ProjectControllerTest
{
    /// <summary>
    /// A mock implementation of the <see cref="IProjectRepository"/> interface
    /// used for unit testing the ProjectController.
    /// </summary>
    private readonly Mock<IProjectRepository> _mockProjectRepository;

    /// <summary>
    /// Mock object for the AutoMapper instance used in ProjectControllerTest class.
    /// </summary>
    private readonly Mock<IMapper> _mockMapper;

    /// <summary>
    /// A mock implementation of the <see cref="ITodoTaskRepository"/> interface used for testing purposes in the ProjectControllerTest class.
    /// </summary>
    private readonly Mock<ITodoTaskRepository> _mockTodoTaskRepository;

    /// <summary>
    /// The ProjectController instance used to perform unit tests on the ProjectController class.
    /// </summary>
    private readonly ProjectController _controller;

    /// <summary>
    /// An instance of the <see cref="IProjectService"/> that provides project-related operations
    /// for the <see cref="ProjectControllerTest"/>.
    /// </summary>
    private readonly IProjectService _projectService;

    /// <summary>
    /// Unit tests for the ProjectController.
    /// </summary>
    public ProjectControllerTest()
    {
        _mockProjectRepository = new Mock<IProjectRepository>();
        _mockTodoTaskRepository = new Mock<ITodoTaskRepository>();
        _mockMapper = new Mock<IMapper>();
        _projectService = new ProjectService(_mockProjectRepository.Object, _mockTodoTaskRepository.Object);
        _controller = new ProjectController(_projectService);
    }

    /// <summary>
    /// Generates a mock list of TodoTask objects for testing purposes.
    /// </summary>
    /// <param name="qtd">The quantity of TodoTask objects to generate.</param>
    /// <returns>A list of TodoTask objects.</returns>
    private List<TodoTask> MockListTasks(int qtd)
    {
        var lstTasks = new List<TodoTask>();

        for (int i = 0; i < qtd; i++)
        {
            var newTaskBD = new TodoTask
            {
                Id = i, Description = "Teste", Priority = TodoTaskPriorityEnum.High,
                Status = TodoTaskStatusEnum.InProgress, Title = "Teste", CreatedAt = DateTime.Now,
                DueDate = DateTime.Now.AddDays(5), ProjectId = 5, UserId = 1
            };

            lstTasks.Add(newTaskBD);
        }

        var tasksInProjectResult = Result<List<TodoTask>>.Success(lstTasks);
        _mockTodoTaskRepository.Setup(repository => repository.GetTasksByProjectIdAsync(5))
            .ReturnsAsync(tasksInProjectResult.Value);

        return lstTasks;
    }

    /// <summary>
    /// Ensures that the GetProjectById action returns an Ok result when a project with the specified ID exists.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous test operation.
    /// </returns>
    [Fact]
    public async Task GetProjectById_ShouldReturnOk_WhenProjectExists()
    {
        // Arrange
        int projectId = 1;
        var project = new Project
        {
            Id = projectId,
            Name = "Test Project",
            Description = "Test Description"
        };

        // Configurando o repositório para retornar um projeto existente
        _mockProjectRepository.Setup(repo => repo.GetProjectByIdAsync(projectId))
            .ReturnsAsync(project);

        // Configurando o serviço com o repositório mock
        var projectService = new ProjectService(_mockProjectRepository.Object, _mockTodoTaskRepository.Object);
        var controller = new ProjectController(projectService);

        // Act
        var result = await controller.GetProjectById(projectId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);

        var returnedProject = okResult.Value as Project;
        Assert.NotNull(returnedProject);
        Assert.Equal(projectId, returnedProject.Id);
        Assert.Equal("Test Project", returnedProject.Name);
        Assert.Equal("Test Description", returnedProject.Description);
    }

    /// <summary>
    /// Verifies that the GetProjectById method returns a NotFound result
    /// when the specified project does not exist.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous test operation. The task result contains
    /// an assertion that verifies the result is of type NotFoundObjectResult.
    /// </returns>
    [Fact]
    public async Task GetProjectById_ShouldReturnNotFound_WhenProjectDoesNotExist()
    {
        // Arrange
        int projectId = 1;
        
        _mockProjectRepository.Setup(repo => repo.GetProjectByIdAsync(projectId))
            .ReturnsAsync((Project)null);
        
        var projectService = new ProjectService(_mockProjectRepository.Object, _mockTodoTaskRepository.Object);
        var controller = new ProjectController(projectService);

        // Act
        var result = await controller.GetProjectById(projectId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    /// <summary>
    /// Tests that GetProjectsByUserId returns an Ok result when projects exist for the specified user.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the OkObjectResult with a list of projects.
    /// </returns>
    [Fact]
    public async Task GetProjectByUserId_ShouldReturnOk_WhenProjectExists()
    {
        // Arrange
        int userId = 1;
        var project = new Project
        {
            Id = 1,
            Name = "Test Project",
            Description = "Test Description"
        };
        
        var lstProjects = new List<Project>();
        lstProjects.Add(project);

        // Configurando o repositório para retornar um projeto existente
        _mockProjectRepository.Setup(repo => repo.GetProjectsByUserIdAsync(userId))
            .ReturnsAsync(lstProjects);

        // Act
        var result = await _controller.GetProjectsByUserId(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);

        var returnedProject = okResult.Value as List<Project>;
        Assert.NotNull(returnedProject);
        Assert.Equal(project.Id, returnedProject[0].Id);
        Assert.Equal("Test Project", returnedProject[0].Name);
        Assert.Equal("Test Description", returnedProject[0].Description);
    }

    /// <summary>
    /// Ensures that the GetProjectsByUserId method returns a NotFound result
    /// when there are no tasks associated with the specified user ID.
    /// </summary>
    /// <returns>A NotFoundObjectResult if no projects are found for the user.</returns>
    [Fact]
    public async Task GetProjecByUserid_ReturnsNotFound_WhenTaskDoesNotExist()
    {
        // Arrange
        var userId = 1;
        _mockProjectRepository.Setup(repository => repository.GetProjectsByUserIdAsync(userId))
            .ReturnsAsync(new List<Project>());

        // Act
        var result = await _controller.GetProjectsByUserId(userId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    /// <summary>
    /// Tests if creating a project successfully returns an Ok result.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task CreateProject_ShouldReturnOk_WhenProjectWasCreatedSuccess()
    {
        var project = new Project();
        project.Name = "Test Project";
        project.Description = "Test Description";
        project.UserId = 1;
        project.StartDate = DateTime.Now;
        
        _mockProjectRepository.Setup(repository=> repository.CreateProjectAsync(project)).ReturnsAsync(project.UserId);
        
        //Act
        var result = await _controller.CreateProject(project) as CreatedAtActionResult;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(nameof(_controller.GetProjectById), result.ActionName);
        Assert.Equal(project, result.Value);
    }

    /// <summary>
    /// Verifies that updating a project returns an Ok result when the project is successfully updated.
    /// </summary>
    /// <returns>NoContentResult if the project update is successful.</returns>
    [Fact]
    public async Task UpdateProject_ShouldReturnOk_WhenProjectWasUpdateSuccess()
    {
        var project = new Project();
        project.Id = 1;
        project.Name = "Test Project";
        project.Description = "Test Description";
        project.UserId = 1;
        project.StartDate = DateTime.Now;

        _mockProjectRepository.Setup(repository => repository.GetProjectByIdAsync(1)).ReturnsAsync(project);
        _mockProjectRepository.Setup(repository => repository.UpdateProjectAsync(project)).Returns(Task.CompletedTask);
        
        //Act
        var result = await _controller.UpdateProject(1, project) as NoContentResult;
        
        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    /// <summary>
    /// Verifies that the UpdateProject method returns a NotFound result when the specified
    /// project is not found in the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task UpdateProject_ShouldNotFound_WhenProjectWasNotFound()
    {
        var project = new Project();
        project.Id = 1;
        project.Name = "Test Project";
        project.Description = "Test Description";
        project.UserId = 1;
        project.StartDate = DateTime.Now;

        _mockProjectRepository.Setup(repository => repository.GetProjectByIdAsync(2)).ReturnsAsync(project);
        _mockProjectRepository.Setup(repository => repository.UpdateProjectAsync(project)).Returns(Task.CompletedTask);
        
        //Act
        var result = await _controller.UpdateProject(1, project) as NotFoundObjectResult;
        
        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.Equal("Project not found.", notFoundResult.Value);
    }

    /// <summary>
    /// Tests if deleting a project returns an Ok result when the project was successfully deleted.
    /// </summary>
    /// <returns>An asynchronous task that represents the unit test.</returns>
    [Fact]
    public async Task DeleteProject_ShouldReturnOk_WhenProjectWasDeletedSuccess()
    {
        var project = new Project();
        project.Id = 1;
        project.Name = "Test Project";
        project.Description = "Test Description";
        project.UserId = 1;
        project.StartDate = DateTime.Now;

        _mockProjectRepository.Setup(repository => repository.GetProjectByIdAsync(1)).ReturnsAsync(project);
        _mockProjectRepository.Setup(repository => repository.DeleteProjectAsync(1)).Returns(Task.CompletedTask);
        
        _mockTodoTaskRepository.Setup(repository => repository.GetTasksByProjectIdAndStatusAsync(1, 
            TodoTaskStatusEnum.Pending)).ReturnsAsync(MockListTasks(0));
        
        //Act
        var result = await _controller.DeleteProject(1) as NoContentResult;
        
        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    /// <summary>
    /// Ensures that attempting to delete a project with pending tasks returns a BadRequest result.
    /// </summary>
    /// <returns>A BadRequestObjectResult indicating that the project cannot be deleted due to pending tasks.</returns>
    [Fact]
    public async Task DeleteProject_ShouldReturnBadRequest_WhenProjectHaveTasksPending()
    {
        var project = new Project();
        project.Id = 1;
        project.Name = "Test Project";
        project.Description = "Test Description";
        project.UserId = 1;
        project.StartDate = DateTime.Now;

        _mockProjectRepository.Setup(repository => repository.GetProjectByIdAsync(1)).ReturnsAsync(project);
        _mockProjectRepository.Setup(repository => repository.DeleteProjectAsync(1)).Returns(Task.CompletedTask);
        
        _mockTodoTaskRepository.Setup(repository => repository.GetTasksByProjectIdAndStatusAsync(1, 
            TodoTaskStatusEnum.Pending)).ReturnsAsync(MockListTasks(20));
        
        //Act
        var result = await _controller.DeleteProject(1) as BadRequestObjectResult;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("Cannot delete the project because there are pending tasks. Please complete or remove all " +
                     "tasks associated with the project before attempting to delete it.", 
            (result.Value as dynamic).Message);
    }

    /// <summary>
    /// Tests that the DeleteProject method returns a BadRequest result
    /// when the project to be deleted is not found.
    /// </summary>
    /// <returns>
    /// A Task representing the asynchronous operation, with a BadRequest result if the project is not found.
    /// </returns>
    [Fact]
    public async Task DeleteProject_ShouldReturnBadRequest_WhenProjectNotFound()
    {
        var project = new Project();
        project.Id = 1;
        project.Name = "Test Project";
        project.Description = "Test Description";
        project.UserId = 1;
        project.StartDate = DateTime.Now;

        _mockProjectRepository.Setup(repository => repository.GetProjectByIdAsync(2)).ReturnsAsync(project);
        _mockProjectRepository.Setup(repository => repository.DeleteProjectAsync(1)).Returns(Task.CompletedTask);
        
        _mockTodoTaskRepository.Setup(repository => repository.GetTasksByProjectIdAndStatusAsync(1, 
            TodoTaskStatusEnum.Pending)).ReturnsAsync(MockListTasks(20));
        
        //Act
        var result = await _controller.DeleteProject(1) as BadRequestObjectResult;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("Project not found.", (result.Value as dynamic).Message);
    }
}