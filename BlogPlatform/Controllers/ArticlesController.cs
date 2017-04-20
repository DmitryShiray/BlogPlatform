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
    public class ArticlesController : Controller
    {
        private readonly IAuthorizationService authorizationService;
        private IArticlesFilteringService articlesFilteringService;
        private IArticleRatingService articleRatingService;

        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ArticlesController(IAuthorizationService authorizationService,
                                  IArticlesFilteringService articlesFilteringService,
                                  IArticleRatingService articleRatingService)
        {
            this.authorizationService = authorizationService;
            this.articlesFilteringService = articlesFilteringService;
            this.articleRatingService = articleRatingService;
        }
        
        [HttpGet("{page:int=0}/{pageSize=12}")]
        public async Task<IActionResult> GetArticles(int page, int pageSize)
        {
            List<ArticleViewModel> pagedSet = new List<ArticleViewModel>();

            try
            {
                IEnumerable<Article> articles = await articlesFilteringService.GetAllArticles();
                
                IEnumerable<ArticleViewModel> articlesViewModel = Mapper.Map<IEnumerable<Article>, IEnumerable<ArticleViewModel>>(articles);

                return new ObjectResult(articlesViewModel);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
            }

            return new ObjectResult(pagedSet);
        }

        [HttpGet("{articleId:int}/{page:int=0}/{pageSize=12}")]
        public async Task<IActionResult> GetArticle(int articleId, int? page, int? pageSize)
        {
            List<ArticleViewModel> pagedSet = new List<ArticleViewModel>();

            try
            {
                //    if (await authorizationService.AuthorizeAsync(User, Claims.ClaimsPolicyValue))
                //    {
                Article article = await articlesFilteringService.GetArticle(articleId);
                ArticleViewModel articlesViewModel = Mapper.Map<Article, ArticleViewModel>(article);

                return new ObjectResult(articlesViewModel);
                //}
                //else
                //{
                //    CodeResultStatus _codeResult = new CodeResultStatus(401);
                //    return new ObjectResult(_codeResult);
                //}
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
            }

            return new ObjectResult(pagedSet);
        }
    }
}
