using TMX.TaskService.Application.Common.Models;
using TMX.TaskService.Domain.Entities;

namespace TMX.TaskService.WebApi.Services
{
    public interface ITaskManagerService
    {
        Task<IEnumerable<UserTaskDto>> GetUserTasks();
        Task<UserTaskDto> GetUserTaskByTaskId(Guid TaskId);
        Task<UserTask> Create(CreateUserTaskDto taskDto, CancellationToken cancellationToken);
        Task Update(UpdateUserTaskDto taskDto, Guid id, CancellationToken cancellationToken);
        Task Delete(Guid id, CancellationToken cancellationToken);
    }
}
