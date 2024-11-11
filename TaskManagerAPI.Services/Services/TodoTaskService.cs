using TaskManagerAPI.Core.Common;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Core.Enums;
using TaskManagerAPI.Infrastructure.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services.Services;

public class TodoTaskService : ITodoTaskService
{
    private readonly IHistoryService _historyService;
    private readonly ITodoTaskRepository _todoTaskRepository;

    public TodoTaskService(ITodoTaskRepository todoTaskRepository, IHistoryService historyService)
    {
        _todoTaskRepository = todoTaskRepository;
        _historyService = historyService;
    }

    public async Task<TodoTask> GetTodoTaskByIdAsync(int id)
    {
        return await _todoTaskRepository.GetTodoTaskByIdAsync(id);
    }

    public async Task<List<TodoTask>> GetTasksByProjectIdAsync(int projectId)
    {
        return await _todoTaskRepository.GetTasksByProjectIdAsync(projectId);
    }

    public async Task<Result<TodoTask>> CreateTodoTaskAsync(TodoTask task)
    {
        var tasksInProject = await _todoTaskRepository.GetTasksByProjectIdAsync(task.ProjectId);

        if (tasksInProject.Count() >= 20)
            return Result<TodoTask>.Failure("The project has reached the maximum limit of 20 tasks.");

        int id = await _todoTaskRepository.CreateTodoTaskAsync(task);
        task.Id = id;
        await _historyService.AddHistoryEntryAsync(task.Id, "Task created", task);
        
        return Result<TodoTask>.Success(task);
    }

    public async Task UpdateTodoTaskAsync(TodoTask task)
    {
        await _todoTaskRepository.UpdateTodoTaskAsync(task);
        await _historyService.AddHistoryEntryAsync(task.Id, "Task updated.", task);
    }

    public async Task<Result> SoftDeleteTodoTaskAsync(int taskId)
    {
        // Obtenha a tarefa para verificar se ela existe
        var task = await _todoTaskRepository.GetTodoTaskByIdAsync(taskId);

        if (task == null) return Result.Failure("Task not found.");

        // Marcar a tarefa como 'Deleted'
        task.Status = TodoTaskStatusEnum.Deleted;
        await _todoTaskRepository.UpdateTodoTaskAsync(task);

        // Adicionar uma entrada ao histórico informando a deleção
        await _historyService.AddHistoryEntryAsync(task.Id, "Task marked as deleted", task);

        return Result.Success();
    }
}