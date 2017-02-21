namespace BlogPlatform.Domain.Services.Abstract
{
    public interface IArticleRatingService
    {
        double GetRating(int articleId);
        void SetRating(int articleId, int accountId, byte value);
    }
}
