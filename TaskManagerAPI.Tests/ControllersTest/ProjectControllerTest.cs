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

public class ProjectControllerTest
{
    private readonly Mock<IProjectRepository> _mockProjectRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ITodoTaskRepository> _mockTodoTaskRepository;
    private readonly ProjectController _controller;
    private readonly IProjectService _projectService;

    public ProjectControllerTest()
    {
        _mockProjectRepository = new Mock<IProjectRepository>();
        _mockTodoTaskRepository = new Mock<ITodoTaskRepository>();
        _mockMapper = new Mock<IMapper>();
        _projectService = new ProjectService(_mockProjectRepository.Object, _mockTodoTaskRepository.Object);
        _controller = new ProjectController(_projectService);
    }

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