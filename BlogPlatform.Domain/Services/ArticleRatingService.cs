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

        public double GetRatingFromDatabase(int articleId)
        {
            var ratings = context.Ratings.Where(r => r.Article.Id == articleId);
            return ComputeRating(ratings);
        }

        public double ComputeRating(IEnumerable<Rating> ratings)
        {
            if (ratings.Count() == 0)
            {
                return 0;
            }

            return ratings.Sum(r => r.Value) / ratings.Count();
        }

        public void SetRating(int articleId, int accountId, byte value)
        {
            context.Ratings.Add(new Rating() { Value = value, AccountId = accountId, ArticleId = articleId });
        }
    }
}
