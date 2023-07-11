using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TMX.ContextLibrary.Entities;

namespace TMX.ContextLibrary.Interfaces
{
    public interface IAuthDbContext
    {
        DbSet<AppUser> Users { get; set; }
        DbSet<IdentityRole> Roles { get; set; }
        DbSet<IdentityUserRole<string>> UserRoles { get; set; }
        DbSet<IdentityUserClaim<string>> UserClaims { get; set; }
        DbSet<IdentityUserLogin<string>> UserLogins { get; set; }
        DbSet<IdentityUserToken<string>> UserTokens { get; set; }
        DbSet<IdentityRoleClaim<string>> RoleClaims { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

}
