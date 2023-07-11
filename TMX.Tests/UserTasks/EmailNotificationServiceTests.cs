using Microsoft.Extensions.Configuration;
using TMX.TaskService.WebApi.Services;

namespace TMX.Tests.UserTasks
{
    public class EmailNotificationServiceTests
    {
        private readonly EmailNotificationService _sut;

        public EmailNotificationServiceTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            _sut = new EmailNotificationService(configuration);
        }


		[Fact]
		public async Task SentEmailNotification_Succes()
		{
            // Arrange
            var email = "YOUR EMAIL FOR TEST";
            var subject = "Test Subject";
            var body = "Test Body";

            // Act
            await _sut.SentEmailNotificationAsync(email, subject, body);

            // Assert
            // It is necessary to check the mailbox specified in Arrange
        }
    }
}
