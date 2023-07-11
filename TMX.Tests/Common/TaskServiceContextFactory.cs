using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TMX.ContextLibrary.Contexts;
using TMX.TaskService.Domain.Entities;
using TMX.TaskService.Domain.Enums;

namespace TMX.Tests.Common
{
    public class TaskServiceContextFactory
    {
        public static Guid UserAId = Guid.NewGuid();
        public static Guid UserBId = Guid.NewGuid();

        public static Guid TaskId_1 = Guid.NewGuid();
        public static Guid TaskId_2 = Guid.NewGuid();

        public static Guid UserTaskIdForUpdate = Guid.NewGuid();
        public static Guid UserTaskIdForDelete = Guid.NewGuid();

        public static TaskServiceDbContext CreateDbContext(string nameDb)
        {
            
            var options = new DbContextOptionsBuilder<TaskServiceDbContext>()
                .UseInMemoryDatabase(nameDb).Options;

            var context = new TaskServiceDbContext(options);
            context.Database.EnsureCreated();
            context.UserTasks.AddRange(
                new UserTask
                {
                    Title = "Title1",
                    Id = TaskId_1,
                    UserId = UserAId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    DueDate = DateTime.UtcNow.AddHours(20),
                    Description = "Description1",
                    Status = UserTaskStatus.InProgress,
                    Priority = UserTaskPriority.Medium,
                    NotificationSent = false
                },
                new UserTask
                {
                    Title = "Title2",
                    Id = TaskId_2,
                    UserId = UserBId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    DueDate = DateTime.UtcNow.AddHours(20),
                    Description = "Description2",
                    Status = UserTaskStatus.InProgress,
                    Priority = UserTaskPriority.Medium,
                    NotificationSent = false
                },
                new UserTask
                {
                    Title = "Title3",
                    Id = UserTaskIdForDelete,
                    UserId = UserBId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    DueDate = DateTime.UtcNow.AddHours(20),
                    Description = "Description3",
                    Status = UserTaskStatus.InProgress,
                    Priority = UserTaskPriority.Medium,
                    NotificationSent = false
                },
                new UserTask
                {
                    Title = "Title4",
                    Id = UserTaskIdForUpdate,
                    UserId = UserAId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    DueDate = DateTime.UtcNow.AddHours(20),
                    Description = "Description4",
                    Status = UserTaskStatus.InProgress,
                    Priority = UserTaskPriority.Medium,
                    NotificationSent = true
                });
            context.SaveChanges();
            return context;
        }

 
        public static IHttpContextAccessor CreateHttpContextAccessor(Guid UserId)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, UserId.ToString())
            };

            var identity = new ClaimsIdentity(claims, "TestAuthenticationType");
            var contextUser = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext()
            {
                User = contextUser
            };

            var contextAccessor = new HttpContextAccessor
            {
                HttpContext = httpContext
            };
            return contextAccessor;
        }

        public static void Destroy(TaskServiceDbContext taskSerivceDbContext)
        {
            taskSerivceDbContext.Database.EnsureDeleted();
            taskSerivceDbContext.Dispose();
            
        }
    }
}
