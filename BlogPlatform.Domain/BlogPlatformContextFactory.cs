﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace BlogPlatform.Domain
{
    //For migration needs
    public class BlogPlatformContextFactory : IDbContextFactory<BlogPlatformContext>
    {
        public IConfigurationRoot Configuration { get; }
        public BlogPlatformContext Create(DbContextFactoryOptions options)
        {
            var builder = new DbContextOptionsBuilder<BlogPlatformContext>();
            builder.UseSqlServer("Server=DESKTOP-2O1M9JS;Database=BlogPlatform;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new BlogPlatformContext(builder.Options);
        }
    }
}
