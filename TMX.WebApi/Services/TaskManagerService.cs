using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TMX.ContextLibrary.Contexts;
using TMX.ContextLibrary.Interfaces;
using TMX.TaskService.Application.Common.Exceptions;
using TMX.TaskService.Application.Common.Models;
using TMX.TaskService.Domain.Entities;

namespace TMX.TaskService.WebApi.Services
{
    public class TaskManagerService : BaseService, ITaskManagerService
    {
        private readonly ITaskServiceDbContext _dbContext;
        private readonly IMapper _mapper;

        public TaskManagerService(ITaskServiceDbContext dbContext, IMapper mapper,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UserTaskDto>> GetUserTasks()
        {
            var userTasks = await _dbContext.UserTasks
                .Where(x => x.UserId == UserId)
                .ToListAsync();
            var userTasksDto = _mapper.Map<List<UserTaskDto>>(userTasks);
            return userTasksDto;

        }

        public async Task<UserTaskDto> GetUserTaskByTaskId(Guid userTaskId)
        {
            var userTask = await _dbContext.UserTasks
                .Where(x => x.UserId == UserId)
                .FirstOrDefaultAsync(x => x.Id == userTaskId) ??
                throw new NotFoundException(nameof(UserTask), userTaskId);
                
            var userTaskDto = _mapper.Map<UserTaskDto>(userTask);
            return userTaskDto;
        }

        public async Task<UserTask> Create(CreateUserTaskDto taskDto, CancellationToken cancellationToken)
        {
            var userTask = _mapper.Map<UserTask>(taskDto);
            userTask.UserId = UserId;
            userTask.CreatedAt = DateTime.UtcNow;

            await _dbContext.UserTasks.AddAsync(userTask, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return userTask;
        }   

        public async Task Update(UpdateUserTaskDto taskDto, Guid userTaskId, CancellationToken cancellationToken)
        {
            var userTask = await _dbContext.UserTasks
                .FirstOrDefaultAsync(x => x.Id == userTaskId && x.UserId == UserId, 
                cancellationToken) ?? 
                throw new NotFoundException(nameof(UserTask), userTaskId);

            _mapper.Map(taskDto, userTask);
            userTask.UpdatedAt = DateTime.UtcNow;

            bool isTaskDueInLessThanOneDay = UserTaskMonitoringService.TaskDueDateCheck(userTask);
            if (isTaskDueInLessThanOneDay)
            {
                userTask.NotificationSent = false;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

        }
        public async Task Delete(Guid userTaskId, CancellationToken cancellationToken)
        {
            var userTask = await _dbContext.UserTasks
                .FirstOrDefaultAsync(x => x.Id == userTaskId, cancellationToken) ??
                throw new NotFoundException(nameof(UserTask), userTaskId);

            _dbContext.UserTasks.Remove(userTask);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
