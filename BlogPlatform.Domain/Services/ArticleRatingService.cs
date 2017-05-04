using System.Linq;
using BlogPlatform.Domain.Entities;
using BlogPlatform.Domain.Services.Abstract;
using System.Collections.Generic;

namespace BlogPlatform.Domain.Services
{
    public class ArticleRatingService : IArticleRatingService
    {
        private BlogPlatformContext context;

        public ArticleRatingService(BlogPlatformContext context)
        {
            this.context = context;
        }

        public async void SetRating(Rating rating)
        {
            await context.Ratings.AddAsync(rating);
            context.SaveChanges();
        }
    }
}
