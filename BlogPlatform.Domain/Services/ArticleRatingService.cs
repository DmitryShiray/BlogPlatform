using System.Linq;
using BlogPlatform.Domain.Entities;
using BlogPlatform.Domain.Services.Abstract;

namespace BlogPlatform.Domain.Services
{
    public class ArticleRatingService : IArticleRatingService
    {
        private BlogPlatformContext context;

        public ArticleRatingService(BlogPlatformContext context)
        {
            this.context = context;
        }

        public double GetRating(int articleId)
        {
            var articles = context.Ratings.Where(r => r.Article.Id == articleId);
            return articles.Sum(r => r.Value) / articles.Count();
        }

        public void SetRating(int articleId, int accountId, byte value)
        {
            context.Ratings.Add(new Rating() { Value = value, AccountId = accountId, ArticleId = articleId });
        }
    }
}
