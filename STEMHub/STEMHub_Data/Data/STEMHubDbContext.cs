using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using STEMHub.STEMHub_Data.Entities;

namespace STEMHub.STEMHub_Data.Data
{
    public class STEMHubDbContext : IdentityDbContext<ApplicationUser>
    {
        public STEMHubDbContext(DbContextOptions<STEMHubDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);


            builder.Entity<Lesson>()
                .HasOne(e => e.Video)
                .WithOne(e => e.Lesson)
                .HasForeignKey<Video>(e => e.LessonId)
                .IsRequired(false);
        }


        public DbSet<STEM> STEM { get; set; }
        public DbSet<Banner> Banner { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Lesson> Lesson { get; set; }
        public DbSet<NewspaperArticle> NewspaperArticle { get; set; }
        public DbSet<Topic> Topic { get; set; }
        public DbSet<Video> Video { get; set; }

        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData
            (
                new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole() { Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" }
            );
        }

    }
}
