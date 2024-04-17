using Microsoft.EntityFrameworkCore;
using UserApplication.Models;

namespace UserApplication
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext() : base() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }  
        public DbSet<UserSocialURLs> UserSocialURLs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserSocialURLs>()
                .HasOne(p => p.User)
                .WithMany(a => a.SocialMediaProfiles)
                .HasForeignKey(p => p.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
        //public DbSet<UserApplication.Models.ChangePassword> ChangePassword { get; set; } = default!;

    }
}
