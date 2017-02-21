using System.Collections.Generic;
using System.Linq;
using BlogPlatform.Domain.Entities;
using BlogPlatform.Domain.Services.Abstract;

namespace BlogPlatform.Domain.Services
{
    public class ArticlesFilteringService : IArticlesFilteringService
    {
        private BlogPlatformContext context;

        public ArticlesFilteringService(BlogPlatformContext context)
        {
            this.context = context;
        }

        public List<Article> GetAllArticles()
        {
            return context.Articles.ToList();
        }

        public List<Article> GetArticles(string searchText)
        {
            return context.Articles.Where(article => article.Title.Contains(searchText)).ToList();
        }
    }
}
