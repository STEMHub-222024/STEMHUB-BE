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
            builder.Entity<Comment>()
                .Property(c => c.Type)
                .HasConversion(
                    v => v.ToString(), // Chuyển enum thành chuỗi
                    v => (CommentType)Enum.Parse(typeof(CommentType), v) // Chuyển chuỗi thành enum
                );
        }


        public DbSet<STEM> STEM { get; set; }
        public DbSet<Banner> Banner { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Lesson> Lesson { get; set; }
        public DbSet<NewspaperArticle> NewspaperArticle { get; set; }
        public DbSet<Topic> Topic { get; set; }
        public DbSet<Video> Video { get; set; }
        public DbSet<Owner> Owner { get; set; }
        public DbSet<Scientist> Scientist { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionSearch> QuestionSearches { get; set; }
        public DbSet<Like> Like { get; set; }
        public DbSet<Parts> Parts { get; set; }
        public DbSet<Search> Search { get; set; }

        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData
            (
                new IdentityRole()
                {
                    Id = "d6941b2d-f5ae-4b88-afa2-0bfa7361d5aa",
                    Name = "Admin",
                    ConcurrencyStamp = "1",
                    NormalizedName = "Admin"
                },
                new IdentityRole()
                {
                    Id = "17e91c5b-c0f3-4e32-8e94-6bdf56ec18d1",
                    Name = "User",
                    ConcurrencyStamp = "2",
                    NormalizedName = "User"
                }
            );
        }

    }
}
