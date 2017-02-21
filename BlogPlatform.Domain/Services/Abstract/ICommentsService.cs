using System.Collections.Generic;
using System.Threading.Tasks;
using BlogPlatform.Domain.Entities;

namespace BlogPlatform.Domain.Services.Abstract
{
    public interface ICommentsService
    {
        Task<List<Comment>> GetComments(int articleId);
        Task<int> GetCommentsNumber(int articleId);
        void AddComment(int articleId, int accountId, string comment);
    }
}
