using AutoMapper;
using BlogPlatform.Domain.Entities;
using BlogPlatform.ViewModels;
using System.Linq;

namespace BlogPlatform.Infrastructure.AutoMapperResolvers
{
    public class ArticleRatingResolver : IValueResolver<Article, ArticleViewModel, double>
    {
        public double Resolve(Article source, ArticleViewModel destination, double destMember, ResolutionContext context)
        {
            if (source.Ratings.Count == 0)
            {
                return 0;
            }

            return source.Ratings.Sum(r => r.Value) / source.Ratings.Count;
        }
    }
}
