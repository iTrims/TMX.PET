using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMX.ContextLibrary.Contexts;
using TMX.ContextLibrary.Interfaces;

namespace TMX.TaskService.Persistence
{
    public static class Extensions
    {
        public static IServiceCollection AddTaskServiceDbContext
            (this IServiceCollection service, string connectionString)
        {
            service.AddDbContext<TaskServiceDbContext>(options =>
            {
                options.UseNpgsql(connectionString, 
                    x => x.MigrationsAssembly("TMX.TaskService.WebApi"));
                
            });

            service.AddScoped<ITaskServiceDbContext, TaskServiceDbContext>();
            return service;
        }

        public static IServiceCollection AddAuthDbContext
            (this IServiceCollection service, string connectionString)
        {
            service.AddDbContext<AuthDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
            service.AddScoped<IAuthDbContext, AuthDbContext>();
            return service;
        }
    }
}
