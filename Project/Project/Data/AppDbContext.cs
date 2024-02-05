using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Models.AppModels;

namespace Project.Data
{

    public class AppDbContext : IdentityDbContext<User, IdentityRole, string,
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


        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Posts> Posts { get; set; }

        public DbSet<Comments> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //One to One
            modelBuilder.Entity<UserProfile>()
                        .HasIndex(x => x.DisplayedUsername)
                        .IsUnique();

            modelBuilder.Entity<UserProfile>()
                        .HasOne(u => u.User)
                        .WithOne(usr => usr.userProfile)
                        .HasForeignKey<UserProfile>(u => u.UserId)
                        .OnDelete(DeleteBehavior.Cascade);

            //One to Many
            modelBuilder.Entity<Posts>()
                        .HasOne(usr => usr.UserProfile)
                        .WithMany(post => post.Posts)
                        .HasForeignKey(usr => usr.UserProfileId)
                        .OnDelete(DeleteBehavior.Cascade);


            //Many to Many
            modelBuilder.Entity<Comments>()
                        .HasKey(com => new { com.UserProfileId, com.PostId, com.Id });
            modelBuilder.Entity<Comments>()
                        .HasOne(usr => usr.UserProfile)
                        .WithMany(com => com.Comments)
                        .HasForeignKey(usr => usr.UserProfileId)
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Comments>()
                        .HasOne(post => post.Post)
                        .WithMany(com => com.Comments)
                        .HasForeignKey(post => post.PostId)
                        .OnDelete(DeleteBehavior.Cascade);



            base.OnModelCreating(modelBuilder);
            SeedRoles(modelBuilder);

        }


    }
}

