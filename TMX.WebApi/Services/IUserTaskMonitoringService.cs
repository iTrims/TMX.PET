namespace TMX.TaskService.WebApi.Services
{
    public interface IUserTaskMonitoringService
    {
        Task RunUserTaskMonitoringAndNotification(CancellationToken cancellationToken);
    }
}
