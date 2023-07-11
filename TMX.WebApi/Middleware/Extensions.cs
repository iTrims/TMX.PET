namespace TMX.TaskService.WebApi.Middleware
{
    public static class Extensions
    {
        public static IServiceCollection AddProjectMiddleware(this IServiceCollection services)
        {
            return services.AddScoped<ErrorHandlingMiddleware>();
        }

        public static IApplicationBuilder UseProjectMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ErrorHandlingMiddleware>();
        }

    }
}
