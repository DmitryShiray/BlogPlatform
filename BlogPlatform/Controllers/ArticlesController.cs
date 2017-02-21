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

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PhotoGallery.Controllers
{
    [Route("api/[controller]")]
    public class AlbumsController : Controller
    {
        private readonly IAuthorizationService authorizationService;
        private IArticlesFilteringService articlesFilteringService;

        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AlbumsController(IAuthorizationService authorizationService,
                                IArticlesFilteringService articlesFilteringService)
        {
            this.authorizationService = authorizationService;
            this.articlesFilteringService = articlesFilteringService;
        }

        [HttpGet("getArticles")]
        public async Task<IActionResult> GetArticles()
        {
            List<ArticleViewModel> pagedSet = new List<ArticleViewModel>();

            try
            {
                if (await authorizationService.AuthorizeAsync(User, "AdminOnly"))
                {
                    List<Article> articles = articlesFilteringService.GetAllArticles().OrderBy(a => a.Id).ToList();

                    IEnumerable<ArticleViewModel> articlesVM = Mapper.Map<IEnumerable<Article>, IEnumerable<ArticleViewModel>>(articles);

                    return new ObjectResult(articlesVM);
                }
                else
                {
                    CodeResultStatus _codeResult = new CodeResultStatus(401);
                    return new ObjectResult(_codeResult);
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
            }

            return new ObjectResult(pagedSet);
        }
    }
}
