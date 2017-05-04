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
using Microsoft.Extensions.Caching.Memory;
using BlogPlatform.Infrastructure.Result;

namespace BlogPlatform.Controllers
{
    [Route("api/[controller]")]
    public class ArticlesController : BaseController
    {
        private IArticlesFilteringService articlesFilteringService;
        private IArticleRatingService articleRatingService;

        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ArticlesController(IAuthorizationService authorizationService,
                                  IAccountService accountService,
                                  IMemoryCache memoryCache,
                                  IArticlesFilteringService articlesFilteringService,
                                  IArticleRatingService articleRatingService)
            : base(accountService, authorizationService, memoryCache)
        {
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

        [HttpGet("article/{articleId:int}")]
        public async Task<IActionResult> GetArticle(int articleId)
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

        [HttpPost("setRating")]
        public async Task<IActionResult> SetArticleRating([FromBody] RatingViewModel ratingViewModel)
        {
            BaseResult setRatingResult = null;

            try
            {
                Rating rating = Mapper.Map<RatingViewModel, Rating>(ratingViewModel);
                rating.Account = await GetCurrentUserAccount();

                articleRatingService.SetRating(rating);

                setRatingResult = new BaseResult()
                {
                    Succeeded = true
                };
            }
            catch (Exception exception)
            {
                Logger.Error(exception);

                setRatingResult = new BaseResult()
                {
                    Succeeded = false,
                    Message = "Failed to set rating " + exception.Message
                };
            }

            return new ObjectResult(setRatingResult);
        }
    }
}
