using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog.Web;
using Swashbuckle.AspNetCore.SwaggerUI;
using TMX.ContextLibrary.Contexts;
using TMX.TaskService.Application.Common.Mappings;
using TMX.TaskService.Persistence;
using TMX.TaskService.WebApi.Middleware;
using TMX.TaskService.WebApi.Services;

namespace TMX.TaskService.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services
                .AddAuthentication(config =>
                {
                    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
               .AddJwtBearer(options =>
               {
                   options.Authority = "https://localhost:7002";
                   options.Audience = "TMXWebApi";
                   options.RequireHttpsMetadata = false;
               });

            builder.Services.AddAutoMapper(typeof(UserTaskProfile));

            builder.Services.AddTaskServiceDbContext(builder.Configuration
                .GetConnectionString("TaskServiceDbString"));
            builder.Services.AddAuthDbContext(builder.Configuration.GetConnectionString("AuthDbString"));
            
            
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings();

            builder.Services.AddHangfire(x
                => x.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("HangFireDbString")));
            builder.Services.AddHangfireServer();

            builder.Services.AddCors(options =>
            {
                //AllowAll just for example, need to change for real project.
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                });
            });

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://localhost:7002/connect/authorize"),
                            TokenUrl = new Uri("https://localhost:7002/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                {"TMXWebApi", "Demo API - full access"}
                            }
                        }
                    }
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            }
                        },
                        new List<string>()
                    }
                });
            });

            builder.Services.AddProjectServices();
            builder.Services.AddProjectMiddleware();

            builder.Host.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
            })
                .UseNLog();

            var app = builder.Build();

            app.UseHangfireTaskMonitoring();
            app.MapHangfireDashboard();
            
            using (var scope = app.Services.CreateScope())
            {   
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var context = serviceProvider.GetRequiredService<TaskServiceDbContext>();
                    DbInitializer.DbInitialize(context);
                }
                catch (Exception ex)
                {
                    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occured while app initialization.");
                }
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.OAuthClientId("tmx-web-app");
                    c.OAuthAppName("TMX Web");
                    c.OAuthUsePkce();
                });
            }

            
            app.UseProjectMiddleware();

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}