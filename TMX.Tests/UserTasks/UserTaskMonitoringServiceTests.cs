using Moq;
using TMX.ContextLibrary.Contexts;
using TMX.TaskService.WebApi.Services;
using TMX.Tests.Common;

namespace TMX.Tests.UserTasks
{
    public class UserTaskMonitoringServiceTests
    {
        private readonly UserTaskMonitoringService _sut;
        private readonly AuthDbContext _authContext = AuthDbContextFactory.CreateAuthDbContext();
        private readonly TaskServiceDbContext _taskServiceContext = TaskServiceContextFactory.CreateDbContext("Second");
        private readonly Mock<IEmailNotificationService> _emailServiceMock = new();

        public UserTaskMonitoringServiceTests()
        {
            _sut = new UserTaskMonitoringService(_emailServiceMock.Object, _taskServiceContext, 
                _authContext);
        }


        [Fact]
        public async Task RunUserTaskMonitoringAndNotification_Success()
        {
            // Arrange
            _emailServiceMock.Setup(x => x.SentEmailNotificationAsync(It.IsAny<string>(), 
                It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            await _sut.RunUserTaskMonitoringAndNotification(CancellationToken.None);

            // Assert
            _emailServiceMock.Verify(x => x.SentEmailNotificationAsync(
                It.IsAny<string>(), "Task deadline approaching", It.IsAny<string>()), 
                Times.Exactly(3));
            Assert.True(_taskServiceContext.UserTasks.All(x => x.NotificationSent &&
            x.DueDate.HasValue && 
            x.DueDate - DateTime.UtcNow < TimeSpan.FromDays(1)));
        }
    }
}
