using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Microsoft.AspNetCore.Identity;
using TMX.TaskService.Application.Common.Validators;

namespace TMX.TaskService.WebApi.Services
{
    public static class Extensions
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CreateUserTaskValidator>(ServiceLifetime.Transient);
            services.AddValidatorsFromAssemblyContaining<UpdateUserTaskValidator>(ServiceLifetime.Transient);
            services.AddMvc();
            services.AddFluentValidationAutoValidation();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ITaskManagerService, TaskManagerService>();
            services.AddScoped<IEmailNotificationService, EmailNotificationService>();
            services.AddScoped<IUserTaskMonitoringService, UserTaskMonitoringService>();
            return services;
        }

        public static void UseHangfireTaskMonitoring(this IApplicationBuilder app)
        {
            var options = new DashboardOptions
            {
                Authorization = new[] { new BasicAuthAuthorizationFilter
                    (new BasicAuthAuthorizationFilterOptions
                        {
                            RequireSsl = true,
                            SslRedirect = false,
                            LoginCaseSensitive = true,
                            Users = new []
                            {
                                new BasicAuthAuthorizationUser
                                {
                                    Login = "admin",
                                    PasswordClear =  "admin"
                                }
                            }
                        }) }
            };
            app.UseHangfireDashboard("/hangfire", options);

            RecurringJob.AddOrUpdate<IUserTaskMonitoringService>("task-monitoring",
                x => x.RunUserTaskMonitoringAndNotification(CancellationToken.None), Cron.Minutely);
        }
    }
}
