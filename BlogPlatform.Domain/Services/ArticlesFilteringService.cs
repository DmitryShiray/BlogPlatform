using System.Collections.Generic;
using System.Linq;
using BlogPlatform.Domain.Entities;
using BlogPlatform.Domain.Services.Abstract;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BlogPlatform.Domain.Services
{
    public class ArticlesFilteringService : IArticlesFilteringService
    {
        private BlogPlatformContext context;

        public ArticlesFilteringService(BlogPlatformContext context)
        {
            this.context = context;
        }

        public async Task<List<Article>> GetAllArticles()
        {
            return await context.Articles.OrderBy(a => a.Id).ToListAsync();
        }

        public async Task<List<Article>> GetArticles(string searchText)
        {
            return await context.Articles.Where(article => article.Title.Contains(searchText)).OrderBy(a => a.Id).ToListAsync();
        }

        public async Task<Article> GetArticle(int articleId)
        {
            return await context.Articles.Where(article => article.Id == articleId).FirstOrDefaultAsync();
        }
    }
}
