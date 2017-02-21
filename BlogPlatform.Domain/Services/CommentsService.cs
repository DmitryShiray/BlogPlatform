using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogPlatform.Domain.Entities;
using BlogPlatform.Domain.Services.Abstract;
using Microsoft.EntityFrameworkCore;

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
            return await context.Comments.Where(c => c.ArticleId == articleId).ToListAsync();
        }

        public async Task<int> GetCommentsNumber(int articleId)
        {
            return await context.Comments.CountAsync(c => c.ArticleId == articleId);
        }

        public void AddComment(int articleId, int accountId, string comment)
        {
            context.Comments.Add(new Comment() { AccountId = accountId, ArticleId = articleId, Value = comment });
            context.SaveChanges();
        }
    }
}
