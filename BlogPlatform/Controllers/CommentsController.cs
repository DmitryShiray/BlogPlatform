using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NLog;
using BlogPlatform.Domain.Services.Abstract;
using BlogPlatform.Domain.Entities;
using BlogPlatform.ViewModels;
using AutoMapper;
using BlogPlatform.Infrastructure.Result;
using Microsoft.Extensions.Caching.Memory;

namespace BlogPlatform.Controllers
{
    [Route("api/[controller]")]
    public class CommentsController : BaseController
    {
        private readonly IArticlesFilteringService articlesFilteringService;
        private ICommentsService commentsService;

        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public CommentsController(IAuthorizationService authorizationService,
                                  IAccountService accountService,
                                  IMemoryCache memoryCache,
                                  IArticlesFilteringService articlesFilteringService,
                                  ICommentsService commentsService)
            : base(accountService, authorizationService, memoryCache)
        {
            this.articlesFilteringService = articlesFilteringService;
            this.commentsService = commentsService;
        }
        
        [HttpGet("{articleId:int}/{page:int=0}/{pageSize=12}")]
        public async Task<IActionResult> GetComments(int articleId, int page, int pageSize)
        {
            List<CommentViewModel> pagedSet = new List<CommentViewModel>();

            try
            {
                List<Comment> comments = await commentsService.GetComments(articleId);

                IEnumerable<CommentViewModel> commentsViewModel = Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentViewModel>>(comments);

                return new ObjectResult(commentsViewModel);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
            }

            return new ObjectResult(pagedSet);
        }

        [HttpPost("addComment")]
        [Authorize]
        public async Task<IActionResult> AddComment([FromBody] CommentViewModel commentViewModel)
        {
            BaseResult addCommentResult = null;

            try
            {
                Comment comment = Mapper.Map<CommentViewModel, Comment>(commentViewModel);
                comment.Account = await GetCurrentUserAccount();

                await commentsService.AddComment(comment);

                addCommentResult = new BaseResult()
                {
                    Succeeded = true
                };
            }
            catch (Exception exception)
            {
                Logger.Error(exception);

                addCommentResult = new BaseResult()
                {
                    Succeeded = false,
                    Message = "Failed to add a comment " + exception.Message
                };
            }

            return new ObjectResult(addCommentResult);
        }
    }
}
