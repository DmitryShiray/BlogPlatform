using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogPlatform.Domain.Entities;
using BlogPlatform.Domain.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using BlogPlatform.Infrastructure.Exceptions;

namespace BlogPlatform.Domain.Services
{
    public class CommentsService : ICommentsService
    {
        private BlogPlatformContext context;

        public CommentsService(BlogPlatformContext context)
        {
            this.context = context;
        }

        public async Task<List<Comment>> GetComments(int articleId)
        {
            return await context.Comments
                .Include(c => c.Account)
                .Where(c => c.ArticleId == articleId)
                .ToListAsync();
        }

        public async Task<int> GetCommentsNumber(int articleId)
        {
            return await context.Comments
                .CountAsync(c => c.ArticleId == articleId);
        }

        public async Task AddComment(Comment comment)
        {
            await context.Comments.AddAsync(comment);
            context.SaveChanges();
        }
    }
}
