using BlogPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogPlatform.Domain
{
    public class BlogPlatformContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public BlogPlatformContext(DbContextOptions<BlogPlatformContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }

            // Account
            modelBuilder.Entity<Account>().Property(p => p.Nickname).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Account>().Property(p => p.FirstName).IsRequired().HasMaxLength(150);
            modelBuilder.Entity<Account>().Property(p => p.LastName).IsRequired().HasMaxLength(150);
            modelBuilder.Entity<Account>().Property(p => p.EmailAddress).IsRequired().HasMaxLength(150);

            // Article
            modelBuilder.Entity<Article>().Property(a => a.Title).HasMaxLength(100);
            modelBuilder.Entity<Article>().HasMany(a => a.Comments).WithOne(p => p.Article);
            modelBuilder.Entity<Article>().HasMany(a => a.Ratings).WithOne(p => p.Article);

            // Rating
            modelBuilder.Entity<Rating>().Property(u => u.Value).IsRequired();
            modelBuilder.Entity<Rating>().Property(u => u.ArticleId).IsRequired();
            modelBuilder.Entity<Rating>().Property(u => u.AccountId).IsRequired();

            // Comment
            modelBuilder.Entity<Comment>().Property(u => u.ArticleId).IsRequired();
            modelBuilder.Entity<Comment>().Property(u => u.AccountId).IsRequired();
        }
    }
}
