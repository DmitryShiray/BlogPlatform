using System.Collections.Generic;
using BlogPlatform.Domain.Entities;
using System.Threading.Tasks;

namespace BlogPlatform.Domain.Services.Abstract
{
    public interface IArticlesFilteringService
    {
        Task<List<Article>> GetAllArticles();

        Task<List<Article>> GetArticles(string searchText);

        Task<Article> GetArticle(int articleId);
    }
}
