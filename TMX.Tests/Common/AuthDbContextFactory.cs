using Microsoft.EntityFrameworkCore;
using TMX.ContextLibrary.Contexts;
using TMX.ContextLibrary.Entities;

namespace TMX.Tests.Common
{
    public class AuthDbContextFactory
    {
        public static AuthDbContext CreateAuthDbContext()
        {
            var options = new DbContextOptionsBuilder<AuthDbContext>()
                .UseInMemoryDatabase("AuthDb").Options;

            var context = new AuthDbContext(options);
            context.Database.EnsureCreated();
            context.Users.AddRange(
                new AppUser
                {
                    Id = TaskServiceContextFactory.UserBId.ToString(),
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    Email = "test1@gmail.com"
                },
                new AppUser
                {
                    Id = TaskServiceContextFactory.UserAId.ToString(),
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    Email = "test2@gmail.com"
                });
            context.SaveChanges();
            return context;
        }

        public static void Destroy(AuthDbContext authDbContext)
        {
            authDbContext.Database.EnsureDeleted();
            authDbContext.Dispose();
        }
    }
}
