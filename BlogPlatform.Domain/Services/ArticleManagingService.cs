using System.Linq;
using BlogPlatform.Domain.Services.Abstract;
using BlogPlatform.Domain.Entities;

namespace BlogPlatform.Domain.Services
{
    public class ArticleManagingService : IArticleManagingService
    {
        private BlogPlatformContext context;

        public ArticleManagingService(BlogPlatformContext context)
        {
            this.context = context;
        }

        public void CreateArticle(int accountId, string title, string content)
        {
            var article = new Entities.Article() { Title = title, Content = content, AccountId = accountId };

            context.Articles.Add(article);
            context.SaveChanges();
        }

        public void DeleteArticle(int articleId)
        {
            var article = context.Articles.FirstOrDefault(a => a.Id == articleId);

            context.Articles.Remove(article);
        }

        public void UpdateArticle(int articleId, string title, string content)
        {
            var article = context.Articles.FirstOrDefault(a => a.Id == articleId);
            article.Title = title;
            article.Content = content;

            context.SaveChanges();
        }
    }
}
