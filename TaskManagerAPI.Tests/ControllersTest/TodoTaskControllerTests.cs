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
    /// <summary>
    /// Contains unit tests for the TodoTaskController class.
    /// </summary>
    public class TodoTaskControllerTests
    {
        /// <summary>
        /// A mock implementation of the ITodoTaskRepository interface used for testing purposes.
        /// </summary>
        private readonly Mock<ITodoTaskRepository> _mockTodoTaskRepository;

        /// <summary>
        /// Mock implementation of the <see cref="IHistoryService"/> used for unit testing purposes.
        /// </summary>
        private readonly Mock<IHistoryService> _mockHistoryService;

        /// <summary>
        /// A mock instance of the <see cref="IMapper"/> interface used for testing purposes in the
        /// <c>TodoTaskControllerTests</c> class. This mock is configured to simulate the behavior of
        /// AutoMapper mappings for unit tests.
        /// </summary>
        private readonly Mock<IMapper> _mockMapper;

        /// <summary>
        /// An instance of the <see cref="TodoTaskController"/> used to manage and test the various functionalities of the TodoTask endpoints.
        /// </summary>
        private readonly TodoTaskController _controller;

        /// <summary>
        /// Unit test class for TodoTaskController, containing methods to test various API endpoints and their expected behaviors.
        /// </summary>
        public TodoTaskControllerTests()
        {
            _mockTodoTaskRepository = new Mock<ITodoTaskRepository>();
            _mockHistoryService = new Mock<IHistoryService>();
            _mockMapper = new Mock<IMapper>();

            var service = new TodoTaskService(_mockTodoTaskRepository.Object, _mockHistoryService.Object);
            _controller = new TodoTaskController(service, _mockMapper.Object);
        }

        /// <summary>
        /// Mocks a Todo task with predefined properties and sets up the task creation
        /// in the mock repository.
        /// </summary>
        /// <returns>
        /// A mocked <see cref="TodoTask"/> with predefined properties.
        /// </returns>
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

        /// <summary>
        /// Mocks a TodoTaskUpdateDTO object with predefined values.
        /// </summary>
        /// <returns>
        /// A TodoTaskUpdateDTO instance with assigned test values.
        /// </returns>
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

        /// <summary>
        /// Creates a list of mocked TodoTask instances.
        /// </summary>
        /// <param name="qtd">The quantity of TodoTask instances to create.</param>
        /// <returns>A list of mocked TodoTask instances.</returns>
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
        /// Tests that GetTasksByProjectId in TodoTaskController returns an OkObjectResult
        /// with a list of TodoTask objects when tasks exist for a given projectId.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains
        /// an OkObjectResult with a list of TodoTask objects if tasks exist, otherwise a NotFound result.
        /// </returns>
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

        /// <summary>
        /// Ensures the GetTasksByProjectId method returns a NotFound result when no tasks exist for a given project ID.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the action result.</returns>
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

        /// <summary>
        /// Verifies that the GetTodoTaskById method returns a NotFound result
        /// when the specified task does not exist in the repository.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Test to verify that the GetTodoTaskById method returns an Ok result with the specified task.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous test operation. The task result contains an OkObjectResult
        /// with the expected TodoTask if the task exists.
        /// </returns>
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

        /// <summary>
        /// Verifies that the CreateTodoTask method returns a CreatedAtActionResult when a task is created successfully.
        /// </summary>
        /// <returns>
        /// A Task representing the asynchronous operation.
        /// </returns>
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

        /// <summary>
        /// Tests that the CreateTodoTask method of the TodoTaskController should return a BadRequest response
        /// when the task creation fails due to reaching maximum task limit.
        /// </summary>
        /// <returns>Task representing the asynchronous operation of the test.</returns>
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

        /// <summary>
        /// Validates that when a Todo task is updated successfully, the API returns a response indicating the task was created.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an assertion that checks if the HTTP status code is 204 (No Content).
        /// </returns>
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

        /// <summary>
        /// Ensures that deleting a TodoTask returns a NoContent result if the deletion is successful.
        /// </summary>
        /// <returns>NoContentResult if deletion is successful.</returns>
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

        /// <summary>
        /// Ensures that the Delete operation returns a NotFound result when the task to be deleted does not exist in the repository.
        /// </summary>
        /// <returns>A NotFoundObjectResult indicating that the task was not found.</returns>
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