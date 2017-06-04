using BlogPlatform.Domain.Entities;
using System.Threading.Tasks;

namespace BlogPlatform.Domain.Services.Abstract
{
    public interface IArticleManagingService
    {
        Task CreateArticle(Article article);

        void DeleteArticle(int accountId, int articleId);

        void UpdateArticle(Article article);

        bool IsArticleOwner(string emailAddress, int articleId);
    }
}
