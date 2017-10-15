using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BlogPlatform.Domain
{
    //For migration needs
    public class BlogPlatformContextFactory : IDesignTimeDbContextFactory<BlogPlatformContext>
    {
        public BlogPlatformContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<BlogPlatformContext>();
            builder.UseSqlServer("Server=DESKTOP-AR7DG7D;Database=BlogPlatform;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new BlogPlatformContext(builder.Options);
        }
    }
}
