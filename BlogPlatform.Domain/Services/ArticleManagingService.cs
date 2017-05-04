﻿using System.Linq;
using BlogPlatform.Domain.Services.Abstract;
using BlogPlatform.Domain.Entities;
using System.Threading.Tasks;
using BlogPlatform.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BlogPlatform.Domain.Services
{
    public class ArticleManagingService : IArticleManagingService
    {
        private BlogPlatformContext context;

        public ArticleManagingService(BlogPlatformContext context)
        {
            this.context = context;
        }

        public async Task CreateArticle(Article article)
        {
            await context.Articles.AddAsync(article);
            context.SaveChanges();
        }

        public void DeleteArticle(int accountId, int articleId)
        {
            var article = context.Articles
                .Include(a => a.Comments)
                .Include(a => a.Ratings)
                .FirstOrDefault(a => a.AccountId == accountId && a.Id == articleId);

            if (article != null)
            {
                context.Articles.Remove(article);
            }
            else
            {
                throw new ServiceException("Wrong article owner id or the specified article doesn't exist");
            }
            context.SaveChanges();
        }

        public void UpdateArticle(Article updatedArticle)
        {
            var article = context.Articles.FirstOrDefault(a => a.Id == updatedArticle.Id);
            article.Title = updatedArticle.Title;
            article.Content = updatedArticle.Content;

            context.SaveChanges();
        }
    }
}
