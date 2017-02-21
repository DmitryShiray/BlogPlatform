using System.Collections.Generic;
using BlogPlatform.Domain.Entities;

namespace BlogPlatform.Domain.Services.Abstract
{
    public interface IArticlesFilteringService
    {
        List<Article> GetAllArticles();

        List<Article> GetArticles(string searchText);
    }
}
