using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagerAPI.Controllers;
using TaskManagerAPI.Core.Common;
using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Core.Enums;
using TaskManagerAPI.Infrastructure.Interfaces;
using TaskManagerAPI.Services.Interfaces;
using TaskManagerAPI.Services.Services;
using Xunit;

namespace TaskManagerAPI.Tests.ControllersTest
{
    public class TodoTaskControllerTests
    {
        private readonly Mock<ITodoTaskRepository> _mockTodoTaskRepository;
        private readonly Mock<IHistoryService> _mockHistoryService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly TodoTaskController _controller;

        public TodoTaskControllerTests()
        {
            _mockTodoTaskRepository = new Mock<ITodoTaskRepository>();
            _mockHistoryService = new Mock<IHistoryService>();
            _mockMapper = new Mock<IMapper>();

            var service = new TodoTaskService(_mockTodoTaskRepository.Object, _mockHistoryService.Object);
            _controller = new TodoTaskController(service, _mockMapper.Object);
        }
        
        private TodoTask MockOneTask()
        {
            var newTask = new TodoTask
            {
                Id = 1, Description = "Teste", Priority = TodoTaskPriorityEnum.High,
                Status = TodoTaskStatusEnum.InProgress, Title = "Teste", CreatedAt = DateTime.Now,
                DueDate = DateTime.Now.AddDays(5), ProjectId = 5, UserId = 1
            };
            
            var serviceResult = Result<int>.Success(newTask.Id);
            _mockTodoTaskRepository.Setup(repository => repository.CreateTodoTaskAsync(newTask))
                .ReturnsAsync(serviceResult.Value);
            
            return newTask;
        }
        
        private TodoTaskUpdateDTO MockOneTaskUpdateDto()
        {
            var newTask = new TodoTaskUpdateDTO
            {
                Id = 1,
                Description = "Teste",
                Status = TodoTaskStatusEnum.InProgress,
                Title = "Teste",
                CreatedAt = DateTime.Now,
                DueDate = DateTime.Now.AddDays(5),
                ProjectId = 5,
                UserId = 1
            };

            // Configurando o comportamento do mock do mapeamento
            var todoTask = new TodoTask
            {
                Id = newTask.Id,
                Description = newTask.Description,
                Status = newTask.Status,
                Title = newTask.Title,
                CreatedAt = newTask.CreatedAt,
                DueDate = newTask.DueDate,
                ProjectId = newTask.ProjectId,
                UserId = newTask.UserId
            };

            // Configurando o mock do mapper para mapear de TodoTaskUpdateDTO para TodoTask
            _mockMapper.Setup(mapper => mapper.Map<TodoTask>(newTask)).Returns(todoTask);

            // Configurando o mock do repositório para simular a execução do método assíncrono de atualização sem retorno
            _mockTodoTaskRepository
                .Setup(repository => repository.UpdateTodoTaskAsync(It.IsAny<TodoTask>()))
                .Returns(Task.CompletedTask);

            return newTask;
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
        public async Task GetTasksByProjectId_ReturnsOkResult_WithListOfTasks()
        {
            // Arrange
            var projectId = 1;
            var fakeTasks = new List<TodoTask>
            {
                new TodoTask { Id = 1, Description = "Task 1" },
                new TodoTask { Id = 2, Description = "Task 2" }
            };
            _mockTodoTaskRepository.Setup(repository => repository.GetTasksByProjectIdAsync(projectId))
                .ReturnsAsync(fakeTasks);

            // Act
            var result = await _controller.GetTasksByProjectId(projectId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnTasks = Assert.IsType<List<TodoTask>>(okResult.Value);
            Assert.Equal(2, returnTasks.Count);
        }

        [Fact]
        public async Task GetTasksByProjectId_ReturnsNotFound_WhenNoTasksExist()
        {
            // Arrange
            var projectId = 1;
            var emptyTasks = new List<TodoTask>();
            _mockTodoTaskRepository.Setup(repository => repository.GetTasksByProjectIdAsync(projectId))
                .ReturnsAsync(emptyTasks);

            // Act
            var result = await _controller.GetTasksByProjectId(projectId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetTodoTaskById_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskId = 1;
            _mockTodoTaskRepository.Setup(repository => repository.GetTodoTaskByIdAsync(taskId))
                .ReturnsAsync((TodoTask)null);

            // Act
            var result = await _controller.GetTodoTaskById(taskId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetTodoTaskById_ReturnsOkResult_WithTask()
        {
            // Arrange
            var taskId = 1;
            var fakeTask = new TodoTask { Id = 1, Description = "Task 1" };
            _mockTodoTaskRepository.Setup(repository => repository.GetTodoTaskByIdAsync(taskId))
                .ReturnsAsync(fakeTask);

            // Act
            var result = await _controller.GetTodoTaskById(taskId);

            // Mapear entidade para DTO
            var expectedTaskDto = new TodoTaskCreateDTO
            {
                Id = fakeTask.Id,
                Description = fakeTask.Description
            };

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnTask = Assert.IsType<TodoTask>(okResult.Value);
            Assert.Equal(expectedTaskDto.Id, returnTask.Id);
            Assert.Equal(expectedTaskDto.Description, returnTask.Description);
        }
        
        [Fact]
        public async Task CreateTodoTask_ShouldReturnCreatedResponse_WhenTaskIsCreatedSuccessfully()
        {
            // Arrange
            var newTask = MockOneTask();
            MockListTasks(2);

            // Act
            var result = await _controller.CreateTodoTask(newTask) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nameof(_controller.GetTodoTaskById), result.ActionName);
            Assert.Equal(newTask, result.Value);
        }

        [Fact]
        public async Task CreateTodoTask_ShouldReturnBadRequest_WhenTaskCreationFails()
        {
            // Arrange
            var newTask = MockOneTask();
            MockListTasks(20);

            // Act
            var result = await _controller.CreateTodoTask(newTask) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("The project has reached the maximum limit of 20 tasks.", (result.Value as dynamic).Message);
        }
        
        [Fact]
        public async Task UpdateTodoTask_ShouldReturnCreatedResponse_WhenTaskIsUpdatedSuccessfully()
        {
            // Arrange
            var newTask = MockOneTaskUpdateDto();
            MockListTasks(2);

            // Act
            var result = await _controller.UpdateTodoTask(1, newTask) as NoContentResult;

            // Assert
            Assert.Equal(204, result.StatusCode);
        }
        
        [Fact]
        public async Task DeleteTodoTask_ShouldReturnNoContent_WhenDeletionIsSuccessful()
        {
            // Arrange
            int taskId = 1;
    
            // Configurando o repositório para retornar uma tarefa existente
            var existingTask = new TodoTask
            {
                Id = taskId,
                Status = TodoTaskStatusEnum.InProgress
            };

            _mockTodoTaskRepository.Setup(repo => repo.GetTodoTaskByIdAsync(taskId))
                .ReturnsAsync(existingTask);

            _mockTodoTaskRepository.Setup(repo => repo.UpdateTodoTaskAsync(It.IsAny<TodoTask>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteTodoTask(taskId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTodoTask_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            int taskId = 1;

            // Configurando o repositório para retornar null, simulando que a tarefa não existe
            _mockTodoTaskRepository.Setup(repo => repo.GetTodoTaskByIdAsync(taskId))
                .ReturnsAsync((TodoTask)null);

            _mockTodoTaskRepository.Setup(repository => repository.DeleteTodoTaskAsync(taskId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteTodoTask(taskId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("Task not found.", notFoundResult.Value);
        }
    }
}