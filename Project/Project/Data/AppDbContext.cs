using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Project.Data
{

    public class ApplicationUserToken : IdentityUserToken<string>
    {
        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime Expires { get; set; }

        public string? Token { get; set; }
    }

    public class AppDbContext : IdentityDbContext<IdentityUser, IdentityRole, string,
        IdentityUserClaim<string>,
        IdentityUserRole<string>,
        IdentityUserLogin<string>,
        IdentityRoleClaim<string>,
        ApplicationUserToken>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData
                (
                new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole() { Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" },
                new IdentityRole() { Name = "HR", ConcurrencyStamp = "3", NormalizedName = "HR" }
                );
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedRoles(modelBuilder);

        }


    }
}

