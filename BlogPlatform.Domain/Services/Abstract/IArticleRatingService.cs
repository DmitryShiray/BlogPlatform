using BlogPlatform.Domain.Entities;
using System.Collections.Generic;

namespace BlogPlatform.Domain.Services.Abstract
{
    public interface IArticleRatingService
    {
        double GetRatingFromDatabase(int articleId);
        double ComputeRating(IEnumerable<Rating> ratings);
        void SetRating(int articleId, int accountId, byte value);
    }
}
