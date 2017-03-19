namespace BlogPlatform.Domain.Services.Abstract
{
    public interface IArticleManagingService
    {
        void CreateArticle(int accountId, string title, string content);
        void DeleteArticle(int articleId);
        void UpdateArticle(int articleId, string title, string content);
    }
}
