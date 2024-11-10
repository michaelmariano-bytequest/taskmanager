using TaskManagerAPI.Common;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Interfaces;

namespace TaskManagerAPI.Services
{
    public class TodoTaskService : ITodoTaskService
    {
        private readonly ITodoTaskRepository _todoTaskRepository;
        private readonly IHistoryService _historyService;

        public TodoTaskService(ITodoTaskRepository todoTaskRepository, IHistoryService historyService)
        {
            _todoTaskRepository = todoTaskRepository;
            _historyService = historyService;
        }

        public async Task<TodoTask> GetTodoTaskByIdAsync(int id)
        {
            return await _todoTaskRepository.GetTodoTaskByIdAsync(id);
        }

        public async Task<IEnumerable<TodoTask>> GetTasksByProjectIdAsync(int projectId)
        {
            return await _todoTaskRepository.GetTasksByProjectIdAsync(projectId);
        }

        public async Task<Result<TodoTask>> CreateTodoTaskAsync(TodoTask task)
        {
            var tasksInProject = await _todoTaskRepository.GetTasksByProjectIdAsync(task.ProjectId);

            if (tasksInProject.Count() >= 20)
            {
                return Result<TodoTask>.Failure("The project has reached the maximum limit of 20 tasks.");
            }

            await _todoTaskRepository.CreateTodoTaskAsync(task);
            await _historyService.AddHistoryEntryAsync(task.Id, "Task created");
            return Result<TodoTask>.Success(task);
        }

        public async Task UpdateTodoTaskAsync(TodoTask task)
        {
            await _todoTaskRepository.UpdateTodoTaskAsync(task);
            await _historyService.AddHistoryEntryAsync(task.Id, "Task updated.");
        }

        public async Task<Result> SoftDeleteTodoTaskAsync(int taskId)
        {
            // Obtenha a tarefa para verificar se ela existe
            var task = await _todoTaskRepository.GetTodoTaskByIdAsync(taskId);

            if (task == null)
            {
                return Result.Failure("Task not found.");
            }

            // Marcar a tarefa como 'Deleted'
            task.Status = "Deleted";
            await _todoTaskRepository.UpdateTodoTaskAsync(task);

            // Adicionar uma entrada ao histórico informando a deleção
            await _historyService.AddHistoryEntryAsync(task.Id, "Task marked as deleted");

            return Result.Success();
        }
    }
}