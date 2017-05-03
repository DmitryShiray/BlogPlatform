using System;
using BlogPlatform.Domain.Entities;
using BlogPlatform.Domain.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogPlatform.Infrastructure.Constants;

namespace BlogPlatform.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IAuthorizationService authorizationService;
        protected readonly IAccountService accountService;
        private readonly IMemoryCache memoryCache;

        public BaseController(IAccountService accountService,
            IAuthorizationService authorizationService,
            IMemoryCache memoryCache)
        {
            this.accountService = accountService;
            this.memoryCache = memoryCache;
            this.authorizationService = authorizationService;
        }

        protected async Task<Account> GetCurrentUserAccount()
        {
            var claim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return null;
            }

            Account account = await memoryCache.GetOrCreateAsync(CacheConstants.CurrentAccountKey, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(CacheConstants.CacheExpirationMinutes);
                return accountService.GetAccountProfileAsync(claim.Value);
            });

            return account;
        }
    }
}
