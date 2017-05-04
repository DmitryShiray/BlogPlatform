using BlogPlatform.Domain.Entities;
using System.Collections.Generic;

namespace BlogPlatform.Domain.Services.Abstract
{
    public interface IArticleRatingService
    {
        void SetRating(Rating rating);
    }
}
