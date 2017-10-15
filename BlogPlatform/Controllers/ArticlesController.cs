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
using BlogPlatform.Infrastructure.Constants;
using BlogPlatform.Infrastructure.Exceptions;

namespace BlogPlatform.Controllers
{
    [Route("api/[controller]")]
    public class ArticlesController : BaseController
    {
        private readonly IArticlesFilteringService articlesFilteringService;
        private readonly IArticleRatingService articleRatingService;
        private readonly IArticleManagingService articleManagingService;
        private readonly ICommentsService commentsService;

        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ArticlesController(IAuthorizationService authorizationService,
                                  IAccountService accountService,
                                  IMemoryCache memoryCache,
                                  IArticlesFilteringService articlesFilteringService,
                                  IArticleRatingService articleRatingService,
                                  IArticleManagingService articleManagingService,
                                  ICommentsService commentsService)
            : base(accountService, authorizationService, memoryCache)
        {
            this.articlesFilteringService = articlesFilteringService;
            this.articleRatingService = articleRatingService;
            this.articleManagingService = articleManagingService;
            this.commentsService = commentsService;
        }

        [HttpGet("{page:int=0}/{pageSize=12}")]
        public async Task<IActionResult> GetArticles(string userEmailAddress, int page, int pageSize)
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

        [HttpGet("{userEmailAddress}/{page:int=0}/{pageSize=12}")]
        public async Task<IActionResult> GetFilteredByAuthorArticles(string userEmailAddress, int page, int pageSize)
        {
            List<ArticleViewModel> pagedSet = new List<ArticleViewModel>();

            try
            {
                Account account = await accountService.GetAccountProfileAsync(userEmailAddress);
                IEnumerable<Article> articles = articles = await articlesFilteringService.GetAllArticlesForAccount(account);

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
                Article article = await articlesFilteringService.GetArticle(articleId);
                ArticleViewModel articlesViewModel = Mapper.Map<Article, ArticleViewModel>(article);

                return new ObjectResult(articlesViewModel);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
            }

            return new ObjectResult(pagedSet);
        }

        [HttpPost("setRating")]
        [Authorize(Policy = Claims.ClaimsAuthorizedUserPolicyName)]
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
        
        [HttpPost("create")]
        [Authorize(Policy = Claims.ClaimsAuthorizedUserPolicyName)]
        public async Task<IActionResult> CreateArticle([FromBody] ArticleViewModel articleViewModel)
        {
            BaseResult setRatingResult = null;

            try
            {
                Article article = Mapper.Map<ArticleViewModel, Article>(articleViewModel);
                article.Account = await GetCurrentUserAccount();

                await articleManagingService.CreateArticle(article);

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
                    Message = "Failed to create article " + exception.Message
                };
            }

            return new ObjectResult(setRatingResult);
        }

        [HttpPost("update")]
        [Authorize(Policy = Claims.ClaimsAuthorizedUserPolicyName)]
        public async Task<IActionResult> UpdateArticle([FromBody] ArticleViewModel articleViewModel)
        {
            BaseResult setRatingResult = null;

            try
            {
                Article article = Mapper.Map<ArticleViewModel, Article>(articleViewModel);
                article.Account = await GetCurrentUserAccount();

                var authorizationResult = await authorizationService.AuthorizeAsync(User, article, Claims.ClaimsArticleOwnerPolicyName);
                if (authorizationResult.Succeeded)
                {
                    articleManagingService.UpdateArticle(article);

                    setRatingResult = new BaseResult()
                    {
                        Succeeded = true
                    };
                }
                else
                {
                    return Forbid();
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception);

                setRatingResult = new BaseResult()
                {
                    Succeeded = false,
                    Message = "Failed to update article " + exception.Message
                };
            }

            return new ObjectResult(setRatingResult);
        }

        [HttpDelete("{articleId:int}")]
        [Authorize(Policy = Claims.ClaimsAuthorizedUserPolicyName)]
        public async Task<IActionResult> DeleteArticle(int articleId)
        {
            BaseResult setRatingResult = null;

            try
            {
                var authorizationResult = await authorizationService.AuthorizeAsync(User, Claims.ClaimsAutorizedRole);
                if (authorizationResult.Succeeded)
                {
                    Account account = await GetCurrentUserAccount();

                    articleManagingService.DeleteArticle(account.Id, articleId);

                    setRatingResult = new BaseResult()
                    {
                        Succeeded = true
                    };
                }
                else
                {
                    CodeResultStatus _codeResult = new CodeResultStatus(401);
                    return new ObjectResult(_codeResult);
                }
            }
            catch (ServiceException exception)
            {
                Logger.Error(exception);

                setRatingResult = new BaseResult()
                {
                    Succeeded = false,
                    Message = "You are not allowed to delete this article"
                };
            }
            catch (Exception exception)
            {
                Logger.Error(exception);

                setRatingResult = new BaseResult()
                {
                    Succeeded = false,
                    Message = "Failed to delete article " + exception.Message
                };
            }

            return new ObjectResult(setRatingResult);
        }
    }
}
