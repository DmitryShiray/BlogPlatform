using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NLog;
using BlogPlatform.Domain.Services.Abstract;
using BlogPlatform.Domain.Entities;
using BlogPlatform.ViewModels;
using BlogPlatform.Infrastructure.Result;
using AutoMapper;
using BlogPlatform.Infrastructure.Constants;

namespace BlogPlatform.Controllers
{
    [Route("api/[controller]")]
    public class CommentsController : Controller
    {
        private readonly IAuthorizationService authorizationService;
        private IArticlesFilteringService articlesFilteringService;
        private ICommentsService commentsService;

        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public CommentsController(IAuthorizationService authorizationService,
                                  IArticlesFilteringService articlesFilteringService,
                                  ICommentsService commentsService)
        {
            this.authorizationService = authorizationService;
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

                IEnumerable<CommentViewModel> articlesViewModel = Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentViewModel>>(comments);

                return new ObjectResult(articlesViewModel);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
            }

            return new ObjectResult(pagedSet);
        }
    }
}
