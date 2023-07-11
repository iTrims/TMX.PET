using Microsoft.EntityFrameworkCore;
using System.Linq;
using TMX.ContextLibrary.Interfaces;
using TMX.TaskService.Domain.Entities;
using TMX.TaskService.Domain.Enums;


namespace TMX.TaskService.WebApi.Services
{
    public class UserTaskMonitoringService : IUserTaskMonitoringService
    {
        private readonly IEmailNotificationService _emailService;
        private readonly ITaskServiceDbContext _taskServiceContext;
        private readonly IAuthDbContext _authDbContext;

        public UserTaskMonitoringService(IEmailNotificationService emailService,
            ITaskServiceDbContext taskServiceDbContext, IAuthDbContext authDbContext)
        {
            _emailService = emailService;
            _taskServiceContext = taskServiceDbContext;
            _authDbContext = authDbContext;
        }

        public async Task RunUserTaskMonitoringAndNotification(CancellationToken cancellationToken)
        {
            var userTasks = await _taskServiceContext.UserTasks
                .Where(x => !x.NotificationSent)
                .ToListAsync(cancellationToken);

            userTasks = userTasks.Where(x=> TaskDueDateCheck(x)).ToList();

            var userIds = userTasks.Select(x => x.UserId.ToString()).Distinct().ToList();
            var users = await _authDbContext.Users
                .Where(x => userIds.Contains(x.Id))
                .ToListAsync(cancellationToken);

            var tasks = new List<Task>();

            foreach (var userTask in userTasks)
            {
                var email = users
                    .Where(x => x.Id == userTask.UserId.ToString())
                    .Select(u => u.Email)
                    .FirstOrDefault();
                if (email == null) continue;
                
                string subject = "Task deadline approaching";
                string message = $"Less than a day left until the completion of your task: {userTask.Title}";

                tasks.Add(_emailService.SentEmailNotificationAsync(email, subject, message));
                userTask.NotificationSent = true;

            }
            await Task.WhenAll(tasks);
            await _taskServiceContext.SaveChangesAsync(cancellationToken);
        }

        public static bool TaskDueDateCheck(UserTask userTask)
        {
            if (userTask.DueDate.HasValue && userTask.Status == UserTaskStatus.InProgress)
            {
                var timeRemaining = userTask.DueDate - DateTime.UtcNow;
                return timeRemaining < TimeSpan.FromDays(1);
            }
            return false;
        }
    }
}
