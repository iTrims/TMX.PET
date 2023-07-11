namespace TMX.TaskService.WebApi.Services
{
    public interface IEmailNotificationService
    {
        Task SentEmailNotificationAsync(string email, string subject, string body);
    }
}
